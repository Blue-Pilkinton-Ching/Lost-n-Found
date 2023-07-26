using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayJoinCode : ButtonBehaviour
{
    private void OnEnable() {
        buttonText.text = MainDependencies.Singleton.NetworkHelper.Lobby.LobbyCode;
    }
    protected override void OnClick()
    {
        GUIUtility.systemCopyBuffer = MainDependencies.Singleton.NetworkHelper.Lobby.LobbyCode;
    }
}
