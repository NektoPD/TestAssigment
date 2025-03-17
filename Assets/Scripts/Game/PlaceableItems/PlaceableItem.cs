using UnityEngine;

namespace Game.PlaceableItems
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlaceableItem : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _unableToPlaceColor;
        [SerializeField] private Color _availableToPlaceColor;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider2D;
        private Transform _transform;
        private ItemType _itemType;
        private PlaceableItemConfig _placeableItemConfig;
        private bool _isPlaced;

        public bool IsPlaceable { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<BoxCollider2D>();
            _defaultColor = _spriteRenderer.color;
            _transform = transform;
            IsPlaceable = true;
            _isPlaced = false;
        }

        public void Initialize(PlaceableItemConfig itemConfig)
        {
            _placeableItemConfig = itemConfig;
            _spriteRenderer.sprite = _placeableItemConfig.Sprite;
            _spriteRenderer.color = _availableToPlaceColor;
            _itemType = _placeableItemConfig.Type;
            _collider2D.size = _spriteRenderer.sprite.bounds.size;
            _collider2D.isTrigger = true;
        }

        public void Initialize(PlaceableItemConfig itemConfig, Vector3 savedPosition)
        {
            Initialize(itemConfig);
            Place(savedPosition);
        }

        public void Reset()
        {
            _collider2D.isTrigger = true;
            _spriteRenderer.color = _defaultColor;
            IsPlaceable = true;
            _itemType = ItemType.None;
        }

        public void Place(Vector3 targetPosition)
        {
            _collider2D.isTrigger = false;
            _transform.position = targetPosition;
            _spriteRenderer.color = _defaultColor;
            IsPlaceable = false;
            _isPlaced = true;
        }

        public PlaceableItemData GetSaveData()
        {
            return new PlaceableItemData
            {
                Position = _transform.position,
                ItemType = _itemType
            };
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_collider2D.isTrigger) return;

            if (!other.TryGetComponent(out PlaceableItem placableItem)) return;
            _spriteRenderer.color = _unableToPlaceColor;
            IsPlaceable = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_collider2D.isTrigger) return;

            if (!other.TryGetComponent(out PlaceableItem placableItem)) return;
            _spriteRenderer.color = _unableToPlaceColor;
            IsPlaceable = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_collider2D.isTrigger) return;

            _spriteRenderer.color = _availableToPlaceColor;
            IsPlaceable = true;
        }
    }
}