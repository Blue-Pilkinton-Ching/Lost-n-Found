using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;
using VivoxUnity;
using System;

public class VivoxManager : MonoBehaviour
{
    ILoginSession loginSession;



    Account account;
    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void Initialize() 
    {
        Client client = new Client();
        client.Initialize();

        VivoxService.Instance.Initialize();

        account = new Account(Guid.NewGuid().ToString());
        loginSession = client.GetLoginSession(account);
        
        //loginSession.BeginLogin(_serverUri, loginSession.GetLoginToken(_tokenKey, _tokenExpiration), ar => SignInCompleteAsyncCallback(ar));
    }

    private void SignInCompleteAsyncCallback(IAsyncResult result) 
    {
        try
        {
            loginSession.EndLogin(result);
        }
        catch
        {
            // Handle error
            throw;
        }
        // At this point, login is successful and other operations can be performed.
    }
}
