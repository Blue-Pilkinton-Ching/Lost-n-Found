using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;
using VivoxUnity;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using Unity.Services.Authentication;

public class VivoxManager : MonoBehaviour
{
    public bool LoggedIn {get; private set;}
    public Action OnLoggedIntoVivox;
    TimeSpan expirationTime = TimeSpan.FromSeconds(90);
    VivoxUnity.Client client;
    ILoginSession loginSession;
    IChannelSession channelSession;
    AccountId accountId;
    ChannelId channelId;
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

        accountId = new AccountId(
            DependencyHolder.Singleton.VivoxCredentials.Issuer, 
            AuthenticationService.Instance.Profile, 
            DependencyHolder.Singleton.VivoxCredentials.Domain);

        loginSession = client.GetLoginSession(accountId);

        // In a build I have to use something other than loginSession.GetLoginToken()

        loginSession.BeginLogin(
            new Uri(DependencyHolder.Singleton.VivoxCredentials.Server), 
            loginSession.GetLoginToken(DependencyHolder.Singleton.VivoxCredentials.TolkenKey, expirationTime), 
            LoginSessionCallback);

        loginSession.PropertyChanged += LoginPropertyChanged;
    }

    private void LoginSessionCallback(IAsyncResult result) 
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

                JoinChannel("balls");

                try
                {
                    OnLoggedIntoVivox.Invoke();
                } catch { }
                
                break;
        }
    }

    public void JoinChannel(string channelName) 
    {
        channelId = new ChannelId(DependencyHolder.Singleton.VivoxCredentials.Issuer, 
            channelName, DependencyHolder.Singleton.VivoxCredentials.Domain,
            ChannelType.Echo);

        channelSession = loginSession.GetChannelSession(channelId);

        channelSession.BeginConnect(true, false, true, 
            channelSession.GetConnectToken(DependencyHolder.Singleton.VivoxCredentials.TolkenKey, expirationTime), 
            callback => JoinChannelCallback(callback));

        channelSession.PropertyChanged += ChannelPropertyChanged;
    }

    public void LeaveChannel() 
    {
        channelSession.Disconnect();
        loginSession.DeleteChannelSession(channelId);
    }

    private void JoinChannelCallback(IAsyncResult result)
    {
        try
        {
            channelSession.EndConnect(result);
        }
        catch
        {
            Debug.LogError("Failed to Join Channel");

            channelSession.PropertyChanged -= ChannelPropertyChanged;
            throw;
        }
        // At this point, joining channel is successful and other operations can be performed.
    }

    private void ChannelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        var source = (IChannelSession)sender;

        switch (source.AudioState)
        {
            case ConnectionState.Disconnecting:
                Debug.Log("Disconnecting from Vivox Channel " + source.Channel.Name);
                break;
            case ConnectionState.Disconnected:
                Debug.Log("Disconnected from Vivox Channel " + source.Channel.Name);
                break;
            case ConnectionState.Connecting:
                Debug.Log("Connecting into Vivox Channel " + source.Channel.Name);
                break;
            case ConnectionState.Connected:

                Debug.Log("Connected into Vivox Channel " + source.Channel.Name);

                try
                {
                    OnLoggedIntoVivox.Invoke();
                }
                catch { }

                break;
        }
    }
}
