using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Zenject;

public class PasteCodeButton : BasicButton
{

    [SerializeField]
    TMP_InputField inputField;
    NetworkHelper networkConnectionData;

    [Inject]
    public void Construct(NetworkHelper networkConnectionData)
    {
        this.networkConnectionData = networkConnectionData;
    }

    protected override async void OnClick()
    {
        string pastedText = GUIUtility.systemCopyBuffer;

        if (pastedText.Length >= 8)
        {
            pastedText = pastedText.Substring(0, 8);
        }

        StringBuilder sb = new StringBuilder(pastedText);

        for (int i = 0; i < pastedText.Length; i++)
        {
            sb[i] = FixCodeText.FixChar(sb[i]);
        }
        inputField.text = sb.ToString();

        bool result = await networkConnectionData.JoinByCode(inputField.text);

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
