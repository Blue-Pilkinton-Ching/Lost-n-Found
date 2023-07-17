using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class JoinRandomButton : BasicButton
{
    NetworkHelper networkConnectionData;

    [Inject]
    public void Construct(NetworkHelper networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }
    protected override async void OnClick()
    {
        bool result = await networkConnectionData.JoinRandom();

        if (result)
        {
            base.OnClick();
        }
        else
        {
            ButtonsFrozen = false;
        }
    }
}
