using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System;

public class DependencyHolder : MonoBehaviour
{
    // Class that holds all Managers and Dependancies for easy accessibility
    // This class should ONLY hold dependencys for other classes, and not contain any functionality

    public static DependencyHolder Singleton;

    [field: Header("Managers & Controllers")]
    [field: SerializeField] public NetworkManager NetworkManager { get; private set; }
    [field: SerializeField] public NetworkHelper NetworkHelper { get; private set; }
    [field: SerializeField] public UnityTransport UnityTransport { get; private set; }
    [field: SerializeField] public GameLoader GameManager { get; private set; }
    [field: SerializeField] public VivoxManager VivoxManager { get; private set; }

    [field: Header("Instantiated Managers & Controllers")]
    public NetworkedClientManager OwnerClientManager { get; private set; }
    public Action OnOwnerClientManagerChange;
    public NetworkedClientManager PartnerClientManager { get; private set; }
    public Action OnPartnerClientManagerChange;

    [field: Header("Settings")]
    [field: SerializeField] public SharedKeys SharedKeys { get; private set; }
    [field: SerializeField] public ButtonSettings ButtonSettings { get; private set; }
    [field: SerializeField] public VivoxCredentials VivoxCredentials { get; private set; }


    [field: Header("Instantiated Component References")]
    [field: SerializeField] public TextMeshProUGUI OwnerUsernameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI PartnerUsernameText { get; private set; }
    public void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this);
    }
    public void SetOwnerClientManager(NetworkedClientManager instance) 
    {
        OwnerClientManager = instance;
        try
        {
            OnOwnerClientManagerChange.Invoke();
        }
        catch {}
    }
    public void SetPartnerClientManager(NetworkedClientManager instance)
    {
        PartnerClientManager = instance;
        try
        {
            OnPartnerClientManagerChange.Invoke();
        }
        catch {}
    }
}
