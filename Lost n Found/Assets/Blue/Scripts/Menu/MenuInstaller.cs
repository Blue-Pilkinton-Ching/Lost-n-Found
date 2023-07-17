using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Unity.Netcode;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<NetworkHelper>().FromNew().AsSingle().NonLazy();
    }
}