using Game.DataProvider;
using ObjectPools;
using UI;
using UnityEngine;
using Zenject;

namespace Game.PlayerComponents
{
    [RequireComponent(typeof(PlayerBuildController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerBuildController _buildController;

        private PlaceableItemPool _itemProvider;
        private BuildSelectionMenu _menu;
        private PlaceableItemsDataProvider _itemsDataProvider;

        [Inject]
        public void Construct(PlaceableItemPool itemProvider, BuildSelectionMenu menu,
            PlaceableItemsDataProvider itemsDataProvider)
        {
            _itemProvider = itemProvider;
            _menu = menu;
            _itemsDataProvider = itemsDataProvider;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _buildController.Initialize(_itemProvider, _menu, _itemsDataProvider);
        }
    }
}