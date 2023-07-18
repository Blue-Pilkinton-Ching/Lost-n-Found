using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class JoinCodeButton : BasicButton
{
    NetworkHelper networkConnectionData;

    [SerializeField]
    private TMP_InputField InputField;

    [Inject]
    public void Construct(NetworkHelper networkConnectionData)
    {
        this.networkConnectionData = networkConnectionData;
    }

    protected override async void OnClick()
    {
        bool result = await networkConnectionData.JoinByCode(InputField.text);

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
