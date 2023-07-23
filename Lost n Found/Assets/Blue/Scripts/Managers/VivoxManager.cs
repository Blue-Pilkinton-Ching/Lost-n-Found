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
    public bool ReadyToJoinVivoxChannel {get; private set;}
    bool joinNewChannelWhenReady = false;
    string channelName;
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
                ReadyToJoinVivoxChannel = true;

                break;
        }
    }

    public void JoinChannelWhenReady(string channelName) 
    {
        LeaveChannel();
        joinNewChannelWhenReady = true;
        this.channelName = channelName;
        TryJoinVivoxChannel();
    }
    private void TryJoinVivoxChannel() 
    {
        if (joinNewChannelWhenReady & ReadyToJoinVivoxChannel)
        {
            JoinChannel();
        }
        joinNewChannelWhenReady = false;
    }

    private void JoinChannel() 
    {
        channelId = new ChannelId(DependencyHolder.Singleton.VivoxCredentials.Issuer, 
            channelName, DependencyHolder.Singleton.VivoxCredentials.Domain,
            ChannelType.NonPositional);

        channelSession = loginSession.GetChannelSession(channelId);

        channelSession.BeginConnect(true, false, true, 
            channelSession.GetConnectToken(DependencyHolder.Singleton.VivoxCredentials.TolkenKey, expirationTime), 
            callback => JoinChannelCallback(callback));

        channelSession.PropertyChanged += ChannelPropertyChanged;
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
            case ConnectionState.Disconnected:
                Debug.Log("Disconnected from Vivox Channel " + source.Channel.Name);

                ReadyToJoinVivoxChannel = true;
                TryJoinVivoxChannel();
                break;
                
            case ConnectionState.Disconnecting:
                Debug.Log("Disconnecting from Vivox Channel " + source.Channel.Name);
                break;

            case ConnectionState.Connecting:
                Debug.Log("Connecting into Vivox Channel " + source.Channel.Name);
                break;

            case ConnectionState.Connected:

                Debug.Log("Connected into Vivox Channel " + source.Channel.Name);

                ReadyToJoinVivoxChannel = false;
                break;
        }
    }
    public void LeaveChannel() 
    {
        try
        {
            channelSession.Disconnect();
            loginSession.DeleteChannelSession(channelId);
        }
        catch{}
    }
}
