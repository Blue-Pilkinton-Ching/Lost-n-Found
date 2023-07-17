using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services;
using Unity.Services.Core;
using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class NetworkConnectionData
{
    public Action<Exception> OnConnectionError;

    bool signedIn = false;
    public NetworkConnectionData()
    {
        OnConnectionError += ConnectionError;
    }
    private void ConnectionError(Exception ex)
    {
        Debug.LogWarning(ex);
    }

    public async Task<bool> AuthenticatePlayer(){
        
        if (signedIn)
        {
            return true;
        }

        signedIn = true;

        Debug.Log("Authenticating");

        var options = new InitializationOptions();

        options.SetProfile(UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString());

        try
        {
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        return true;
    }

    public async Task<bool> JoinRandom() {

        Lobby lobby;

        Debug.Log("Joining Lobby");

        try
        {
            lobby = await Lobbies.Instance.QuickJoinLobbyAsync(); 
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        return true;
    }
}
