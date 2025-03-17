using SaveSystem;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlaceableItemsSaver>()
                .AsSingle()
                .NonLazy();
        }
    }
}