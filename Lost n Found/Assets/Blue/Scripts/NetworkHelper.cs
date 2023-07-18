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
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Zenject;

public class NetworkHelper
{
    public Action<Exception> OnConnectionError;

    public Lobby Lobby {get; private set;} = null;
    public Allocation RelayAllocation {get; private set;} = null;
    public JoinAllocation RelayJoinAllocation {get; private set;} = null;

    bool signedIn = false;

    UnityTransport transport;

    [Inject]
    public NetworkHelper(UnityTransport transport)
    {
        this.transport = transport;

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
        return await JoinAsClient(true);
    }

    public async Task<bool> JoinByCode(string code)
    {
        return await JoinAsClient(false, code);
    }

    private async Task<bool> JoinAsClient(bool random, string code = "")
    {
        if (Lobby != null)
        {
            Debug.Log("Leaving Current Lobby");

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(Lobby.Id, AuthenticationService.Instance.PlayerId);
            }
            catch (System.Exception ex)
            {
                OnConnectionError.Invoke(ex);
                return false;
            }
        }

        Debug.Log("Joining Lobby");

        try
        {
            if (random)
            {
                Lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            }
            else
            {
                Lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code);
            }
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        Debug.Log("Joining Allocation");

        try
        {
            RelayJoinAllocation = await RelayService.Instance.JoinAllocationAsync(Lobby.Data["JoinCode"].Value);
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        transport.SetClientRelayData(RelayJoinAllocation.RelayServer.IpV4,
            (ushort)RelayJoinAllocation.RelayServer.Port,
            RelayJoinAllocation.AllocationIdBytes,
            RelayJoinAllocation.Key,
            RelayJoinAllocation.ConnectionData,
            RelayJoinAllocation.HostConnectionData);

        return true;
    }

    public async Task<bool> Host(bool isPrivate)
    {

        Debug.Log("Creating Relay Allocation");

        try
        {
            RelayAllocation = await RelayService.Instance.CreateAllocationAsync(2);
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        Debug.Log("Getting Allocation Join Code");

        string joinCode;

        try
        {
            joinCode = await RelayService.Instance.GetJoinCodeAsync(RelayAllocation.AllocationId);
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        CreateLobbyOptions options = new CreateLobbyOptions();

        Dictionary<string, DataObject> lobbyOptionsData = new();
        lobbyOptionsData.Add("JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: joinCode));
        options.Data = lobbyOptionsData;
        options.IsPrivate = isPrivate;

        Debug.Log("Creating Lobby");

        try
        {
            Lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", 2, options);
        }
        catch (System.Exception ex)
        {
            OnConnectionError.Invoke(ex);
            return false;
        }

        Debug.Log("Lobby Code: " + Lobby.LobbyCode);

        transport.SetHostRelayData(RelayAllocation.RelayServer.IpV4, 
            (ushort)RelayAllocation.RelayServer.Port, 
            RelayAllocation.AllocationIdBytes, 
            RelayAllocation.Key, 
            RelayAllocation.ConnectionData);

        Debug.Log("Starting Host");

        NetworkManager.Singleton.StartHost();

        return true;
    }

    IEnumerator LobbyHeartBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            LobbyService.Instance.SendHeartbeatPingAsync(Lobby.Id);
        }
    }
}
