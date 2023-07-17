using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;

public class PlayButton : BasicButton
{
    NetworkConnectionData networkConnectionData;
    protected override async void OnClick()
    {
        bool result = await networkConnectionData.AuthenticatePlayer();

        ButtonsFrozen = false;

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
