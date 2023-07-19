using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkedClientManager : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> Username {get; private set;} = 
        new NetworkVariable<FixedString32Bytes>(
            readPerm: NetworkVariableReadPermission.Everyone, 
            writePerm: NetworkVariableWritePermission.Owner);

    private void Awake() {
        Username.OnValueChanged += UpdateUsername;  
    }

    private void UpdateUsername(FixedString32Bytes previous, FixedString32Bytes current)
    {

    }

    public override void OnNetworkSpawn(){
        if (IsOwner)
        {
            Username.Value = PlayerPrefs.GetString(DependencyHolder.Singleton.SharedKeys.UsernameSaveKey, "Noobie" + Random.Range(0, 1000));
        }
        else
        {
            
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
