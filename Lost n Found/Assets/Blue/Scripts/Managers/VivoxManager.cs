using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;

public class VivoxManager : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void Initialize() 
    {
        VivoxService.Instance.Initialize();

        Account account = new Account();
        //LoginSession = Vi
    }
}
