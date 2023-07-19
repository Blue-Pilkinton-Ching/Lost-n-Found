using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinCodeButton : BasicButton
{
    [SerializeField]
    private TMP_InputField InputField;
    protected override async void OnClick()
    {
        bool result = await DependencyHolder.Singleton.NetworkHelper.JoinByCode(InputField.text);

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
