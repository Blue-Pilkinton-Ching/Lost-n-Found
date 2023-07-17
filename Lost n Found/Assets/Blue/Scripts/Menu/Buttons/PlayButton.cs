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

        if (result)
        {
            base.OnClick();
        }
        else
        {
            ButtonsFrozen = false;
        }
    }

    [Inject]
    public void Construct(NetworkConnectionData networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }
}
