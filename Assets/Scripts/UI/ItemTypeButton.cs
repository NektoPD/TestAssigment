using System;
using Game.PlaceableItems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemTypeButton : MonoBehaviour
    {
        [SerializeField] private Image _selectedImage;
        [SerializeField] private Button _button;

        public event Action<ItemTypeButton> ButtonClicked;

        [field: SerializeField] public ItemType HoldableItem { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void Start()
        {
            Unselect();
        }

        public void Unselect()
        {
            _selectedImage.enabled = false;
        }

        public void SetSelected()
        {
            _selectedImage.enabled = true;
        }

        private void OnButtonClicked()
        {
            SetSelected();
            ButtonClicked?.Invoke(this);
        }
    }
}