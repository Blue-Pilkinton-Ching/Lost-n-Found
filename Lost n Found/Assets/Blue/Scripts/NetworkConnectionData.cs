using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services;
using Unity.Services.Core;
using System;
using System.Threading.Tasks;

public class NetworkConnectionData
{
    public Action<Exception> FailedAction;
    public NetworkConnectionData()
    {
        FailedAction += FailedActionMethod;
    }
    private void FailedActionMethod(Exception ex)
    {
        Debug.LogWarning(ex);
    }

    public async Task<bool> AuthenticatePlayer(){

        var options = new InitializationOptions();

        options.SetProfile(new Guid().ToString());

        try
        {
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception ex)
        {
            FailedAction.Invoke(ex);
            return false;
        }

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception ex)
        {
            FailedAction.Invoke(ex);
            return false;
        }
        return true;
    }
}
