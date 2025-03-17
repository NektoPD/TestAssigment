using System.Collections.Generic;
using Game.DataProvider;
using Game.Input;
using Game.PlaceableItems;
using ObjectPools;
using SaveSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Game.PlayerComponents
{
    public class PlayerBuildController : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private GridPlacementController _gridPlacementController;

        private Camera _camera;
        private BuildSelectionMenu _menu;
        private PlaceableItemsDataProvider _itemsDataProvider;
        private ItemType _currentSelectedType = ItemType.None;
        private PlaceableItemPool _itemProvider;
        private PlaceableItemsSaver _itemsSaver;
        private PlaceableItemRegistry _itemRegistry;

        [Inject]
        public void Construct(PlaceableItemsSaver itemsSaver)
        {
            _itemsSaver = itemsSaver;
            _itemRegistry = new PlaceableItemRegistry();
            LoadSavedItems();
        }

        private void OnDisable()
        {
            UnsubscribeFormEvents();
        }

        public void Initialize(PlaceableItemPool itemProvider, BuildSelectionMenu menu,
            PlaceableItemsDataProvider itemsDataProvider)
        {
            _itemProvider = itemProvider;
            _menu = menu;
            _itemsDataProvider = itemsDataProvider;

            _gridPlacementController.Initialize(_inputHandler);
            _gridPlacementController.OnItemRemoved += ReturnItemToPool;

            _camera = Camera.main;

            SubscribeToEvents();
        }

        private void LoadSavedItems()
        {
            List<PlaceableItemData> savedItems = _itemsSaver.GetLoadedItems();

            if (savedItems.Count <= 0)
                return;

            foreach (PlaceableItemData itemData in savedItems)
            {
                PlaceableItemConfig config = _itemsDataProvider.GetDataByType(itemData.ItemType);
                if (config == null)
                {
                    continue;
                }

                PlaceableItem item = _itemProvider.GetItem(itemData.Position);
                if (item == null)
                {
                    continue;
                }

                item.Initialize(config, itemData.Position);
                _itemRegistry.RegisterItem(item);
            }
        }

        private void SavePlacedItems()
        {
            if (_itemsSaver == null) return;

            List<PlaceableItemData> itemsData = _itemRegistry.GetAllItemData();
            _itemsSaver.SaveItems(itemsData);
        }

        private void SubscribeToEvents()
        {
            _menu.ItemSelected += SetItemType;
            _menu.BuildClicked += EnableBuildMode;
            _menu.RemoveClicked += EnableRemoveMode;

            _gridPlacementController.OnItemPlaced += RegisterItem;
            _gridPlacementController.OnItemRemoved += UnregisterItem;
        }

        private void UnsubscribeFormEvents()
        {
            _menu.ItemSelected -= SetItemType;
            _menu.BuildClicked -= EnableBuildMode;
            _menu.RemoveClicked -= EnableRemoveMode;

            _gridPlacementController.OnItemPlaced -= RegisterItem;
            _gridPlacementController.OnItemRemoved -= UnregisterItem;
        }

        private void SetItemType(ItemType type)
        {
            _currentSelectedType = type;
        }

        private void ReturnItemToPool(PlaceableItem item)
        {
            item.Reset();
            _itemProvider.ReturnItem(item);
        }

        private void EnableRemoveMode()
        {
            _inputHandler.EnablePointerDetection();
            _gridPlacementController.EnterRemoveMode();
        }

        private void RegisterItem(PlaceableItem item)
        {
            _itemRegistry.RegisterItem(item);
            SavePlacedItems();
        }

        private void UnregisterItem(PlaceableItem item)
        {
            _itemRegistry.UnregisterItem(item);
            SavePlacedItems();
        }

        private void EnableBuildMode()
        {
            if (_currentSelectedType == ItemType.None)
                return;

            _inputHandler.EnablePointerDetection();
            PlaceableItemConfig selectedItemData = _itemsDataProvider.GetDataByType(_currentSelectedType);
            Vector2 screenPosition = _inputHandler.CurrentPointerPosition;
            Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
            worldPosition.z = 0;

            var item = _itemProvider.GetItem(worldPosition);

            if (item == null) return;
            item.Initialize(selectedItemData);
            _gridPlacementController.StartPlacingItem(item);
        }
    }
}