using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ButtonSettings", menuName = "ScriptableObjects/ButtonSettings", order = 0)]
public class ButtonSettingsInstaller : ScriptableObjectInstaller<ButtonSettingsInstaller> {
    public float FadeDuration = 0.2f;
    public float ClickDelay = 0.1f;
    public Color NormalColor = Color.white;
    public Color HoverColor = Color.grey;
    public Color ClickColor = Color.black;

    public override void InstallBindings()
    {
        Container.Bind<ButtonSettingsInstaller>().FromInstance(this).AsSingle();
    }
}