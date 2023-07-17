using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Threading.Tasks;

public class PlayButton : BasicButton
{
    NetworkHelper networkConnectionData;
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
    public void Construct(NetworkHelper networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }
}
