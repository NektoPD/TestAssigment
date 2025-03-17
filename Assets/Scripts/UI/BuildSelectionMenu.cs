using System;
using Game.PlaceableItems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildSelectionMenu : MonoBehaviour
    {
        [SerializeField] private ItemTypeButton[] _itemTypeButtons;
        [SerializeField] private Button _buildButton;
        [SerializeField] private Button _removeButton;

        private ItemTypeButton _currentTypeButton;

        public event Action<ItemType> ItemSelected;
        public event Action BuildClicked;
        public event Action RemoveClicked;

        private void OnEnable()
        {
            _buildButton.onClick.AddListener(OnBuildClicked);
            _removeButton.onClick.AddListener(OnRemoveClicked);

            foreach (ItemTypeButton itemTypeButton in _itemTypeButtons)
            {
                itemTypeButton.ButtonClicked += OnItemSelected;
            }
        }

        private void OnDisable()
        {
            _buildButton.onClick.RemoveListener(OnBuildClicked);
            _removeButton.onClick.RemoveListener(OnRemoveClicked);

            foreach (ItemTypeButton itemTypeButton in _itemTypeButtons)
            {
                itemTypeButton.ButtonClicked -= OnItemSelected;
            }
        }

        private void OnItemSelected(ItemTypeButton itemTypeButton)
        {
            if (_currentTypeButton != null)
                _currentTypeButton.Unselect();

            _currentTypeButton = itemTypeButton;
            ItemSelected?.Invoke(_currentTypeButton.HoldableItem);
        }

        private void OnBuildClicked()
        {
            BuildClicked?.Invoke();
        }

        private void OnRemoveClicked()
        {
            RemoveClicked?.Invoke();
        }
    }
}