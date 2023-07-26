using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : BasicButton
{
    protected override void OnClick()
    {
        MainDependencies.Singleton.OwnerClientManager.SetReadyStatus(false);
        base.OnClick();
    }
}
