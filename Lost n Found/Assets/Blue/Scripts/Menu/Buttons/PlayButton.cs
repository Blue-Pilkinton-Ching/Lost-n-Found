using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayButton : BasicButton
{
    NetworkConnectionData networkConnectionData;
    protected override async void OnClick()
    {
        bool result = await networkConnectionData.AuthenticatePlayer();

        if (result)
        {
            base.OnClick();
        }
    }

    [Inject]
    public void Construct(NetworkConnectionData networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }
}
