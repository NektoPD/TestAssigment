using Game.DataProvider;
using Game.PlayerComponents;
using ObjectPools;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private PlaceableItemPool _itemPool;
        [SerializeField] private PlaceableItemsDataProvider _dataProvider;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BuildSelectionMenu _buildSelectionMenu;
        [SerializeField] private Grid _grid;

        public override void InstallBindings()
        {
            Container.Bind<PlaceableItemPool>()
                .FromInstance(_itemPool)
                .AsSingle();

            Container.Bind<PlaceableItemsDataProvider>()
                .FromInstance(_dataProvider)
                .AsSingle();

            Container.Bind<BuildSelectionMenu>()
                .FromInstance(_buildSelectionMenu)
                .AsSingle();

            Container.Bind<PlayerController>()
                .FromInstance(_playerController)
                .AsSingle()
                .NonLazy();

            Container.Bind<Grid>()
                .FromInstance(_grid)
                .AsSingle()
                .NonLazy();
        }
    }
}