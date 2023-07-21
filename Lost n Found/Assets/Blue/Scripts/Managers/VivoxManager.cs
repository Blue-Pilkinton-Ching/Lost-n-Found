using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;
using VivoxUnity;
using System;
using System.Threading.Tasks;
using System.ComponentModel;

public class VivoxManager : MonoBehaviour
{
    public bool LoggedIn {get; private set;}
    public Action OnLoggedIntoVivox;

    VivoxUnity.Client client;
    ILoginSession loginSession;
    Account account;
    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit() {
        client.Uninitialize();
    }
    public void Initialize() 
    {
        VivoxService.Instance.Initialize();

        client = new Client();
        client.Uninitialize();
        client.Initialize();

        AccountId accountId = new AccountId(
            DependencyHolder.Singleton.VivoxCredentials.Issuer, 
            Guid.NewGuid().ToString(), 
            DependencyHolder.Singleton.VivoxCredentials.Domain);

        loginSession = client.GetLoginSession(accountId);

        loginSession.BeginLogin(
            new Uri(DependencyHolder.Singleton.VivoxCredentials.Server), 
            loginSession.GetLoginToken(DependencyHolder.Singleton.VivoxCredentials.TolkenKey, TimeSpan.FromSeconds(90)), 
            SignInCompleteAsyncCallback);

        loginSession.PropertyChanged += LoginPropertyChanged;
    }

    private void SignInCompleteAsyncCallback(IAsyncResult result) 
    {
        try
        {
            loginSession.EndLogin(result);
        }
        catch
        {
            Debug.LogError("Failed to Sign in");

            loginSession.PropertyChanged -= LoginPropertyChanged;
            throw;
        }
        // At this point, login is successful and other operations can be performed.
    }

    private void LoginPropertyChanged(object sender, PropertyChangedEventArgs args) 
    {
        var source = (ILoginSession)sender;

        switch (source.State)
        {
            case LoginState.LoggingIn:
                Debug.Log("Logging into Vivox");
                break;
            case LoginState.LoggedIn:

                Debug.Log("Logged into Vivox");
            
                try
                {
                    OnLoggedIntoVivox.Invoke();
                } catch { }
                
                break;
        }
    }
}
