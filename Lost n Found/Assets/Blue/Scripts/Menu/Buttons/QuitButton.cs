using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : ButtonBehaviour
{
    protected override void OnClick()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
