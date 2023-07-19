using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class DependencyHolder : MonoBehaviour
{
    // Class that holds all Managers and Dependancies for easy accessibility
    // This class should ONLY hold dependencys for other classes, and not contain any functionality

    public static DependencyHolder Singleton;
    [field: SerializeField] public NetworkHelper NetworkHelper { get; private set; }
    [field: SerializeField] public SharedKeys SharedKeys { get; private set; }
    [field: SerializeField] public ButtonSettings ButtonSettings { get; private set; }
    [field: SerializeField] public NetworkedClientManager NetworkedClientManager {get; private set;}
    [field: SerializeField] public UnityTransport UnityTransport { get; private set; }
    public void Awake()
    {
        Singleton = this;
    }
}
