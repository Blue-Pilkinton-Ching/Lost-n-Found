using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HostButton : BasicButton
{
    NetworkHelper networkConnectionData;

    [SerializeField]
    private VisibilityButton visibilityButton;

    [Inject]
    public void Construct(NetworkHelper networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }

    protected override async void OnClick()
    {
        bool result = await networkConnectionData.Host(visibilityButton.IsPrivate);

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
