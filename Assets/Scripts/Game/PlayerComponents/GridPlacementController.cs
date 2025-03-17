using System;
using System.Collections;
using Game.Input;
using Game.PlaceableItems;
using UnityEngine;
using Zenject;

namespace Game.PlayerComponents
{
    public class GridPlacementController : MonoBehaviour
    {
        [SerializeField] private LayerMask _gridLayerMask;
        [SerializeField] private LayerMask _placeableItemsLayerMask;

        private Grid _grid;
        private Camera _mainCamera;
        private PlayerInputHandler _inputHandler;
        private PlaceableItem _currentPlacingItem;
        private bool _isPlacingItem = false;
        private bool _isRemovingItem = false;
        private Coroutine _placementCoroutine;
        private Vector3Int _currentCellPosition;

        public event Action<PlaceableItem> OnItemRemoved;
        public event Action<PlaceableItem> OnItemPlaced;

        [Inject]
        public void Construct(Grid grid)
        {
            _grid = grid;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Initialize(PlayerInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _inputHandler.Clicked += OnPlayerClicked;
        }

        private void OnDisable()
        {
            _inputHandler.Clicked -= OnPlayerClicked;
        }

        public void StartPlacingItem(PlaceableItem item)
        {
            ExitRemoveMode();
            StopPlacingItem();

            _currentPlacingItem = item;
            _isPlacingItem = true;
            _placementCoroutine = StartCoroutine(MoveItemWithCursor());
        }

        public void EnterRemoveMode()
        {
            StopPlacingItem();
            _isRemovingItem = true;
        }

        private void ExitRemoveMode()
        {
            _isRemovingItem = false;
        }

        private void StopPlacingItem()
        {
            if (_placementCoroutine != null)
            {
                StopCoroutine(_placementCoroutine);
                _placementCoroutine = null;
            }

            _isPlacingItem = false;
            _currentPlacingItem = null;
        }

        private void OnPlayerClicked()
        {
            if (_isPlacingItem && _currentPlacingItem != null)
            {
                HandleItemPlacement();
            }
            else if (_isRemovingItem)
            {
                HandleItemRemoval();
            }
        }

        private void HandleItemPlacement()
        {
            if (!_isPlacingItem || _currentPlacingItem == null) return;

            if (_currentPlacingItem.IsPlaceable)
            {
                Vector2 pointerPosition = _inputHandler.CurrentPointerPosition;
                Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y,
                    _mainCamera.nearClipPlane));
                Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);
                Vector2 direction = Vector2.zero;

                RaycastHit2D hit = Physics2D.Raycast(worldPos2D, direction, Mathf.Infinity, _gridLayerMask);
                if (hit.collider != null)
                {
                    Vector3 hitPosition = hit.point;
                    Vector3Int cellPosition = _grid.WorldToCell(hitPosition);
                    Vector3 cellCenter = _grid.GetCellCenterWorld(cellPosition);

                    _currentPlacingItem.Place(cellCenter);
                    OnItemPlaced?.Invoke(_currentPlacingItem);

                    _currentPlacingItem = null;
                    _isPlacingItem = false;

                    if (_placementCoroutine != null)
                    {
                        StopCoroutine(_placementCoroutine);
                        _placementCoroutine = null;
                    }
                }
            }
        }

        private void HandleItemRemoval()
        {
            Vector2 pointerPosition = _inputHandler.CurrentPointerPosition;
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y,
                _mainCamera.nearClipPlane));
            Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero, Mathf.Infinity, _placeableItemsLayerMask);
            if (hit.collider != null && hit.collider.TryGetComponent(out PlaceableItem item))
            {
                if (!item.IsPlaceable)
                {
                    OnItemRemoved?.Invoke(item);
                }
            }
        }

        private IEnumerator MoveItemWithCursor()
        {
            while (_isPlacingItem && _currentPlacingItem != null)
            {
                Vector2 cursorPosition = _inputHandler.CurrentPointerPosition;
                Vector3 worldPosition =
                    _mainCamera.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, 0));
                worldPosition.z = 0;

                Vector3Int cellPosition = _grid.WorldToCell(worldPosition);

                Vector3 snappedPosition = _grid.GetCellCenterWorld(cellPosition);

                _currentPlacingItem.transform.position = snappedPosition;

                yield return null;
            }
        }
    }
}