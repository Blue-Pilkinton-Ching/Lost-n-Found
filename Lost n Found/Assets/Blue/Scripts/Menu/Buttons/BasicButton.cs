using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicButton : ButtonBehaviour
{
    [SerializeField]
    GameObject showMenu;

    [SerializeField]
    GameObject hideMenu;

    protected override void Awake()
    {
        freezeButtons = true;
    }
    protected override void OnClick()
    {
        showMenu.SetActive(true);
        hideMenu.SetActive(false);
    }
}
