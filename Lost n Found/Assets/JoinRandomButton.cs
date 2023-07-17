using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class JoinRandomButton : ButtonBehaviour
{

    NetworkConnectionData networkConnectionData;

    [Inject]
    public void Construct(NetworkConnectionData networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override async void OnClick()
    {
        await networkConnectionData.JoinRandom();
        ButtonsFrozen = false;
    }
}
