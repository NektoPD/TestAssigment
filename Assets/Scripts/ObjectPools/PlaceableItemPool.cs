using Game.PlaceableItems;
using UnityEngine;
using System.Collections.Generic;

namespace ObjectPools
{
    public class PlaceableItemPool : MonoBehaviour
    {
        [SerializeField] private PlaceableItem _prefab;
        [SerializeField] private int _initialSize = 30;

        private readonly Queue<PlaceableItem> _pool = new Queue<PlaceableItem>();

        private void Awake()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                PlaceableItem item = Instantiate(_prefab, transform);
                item.gameObject.SetActive(false);
                _pool.Enqueue(item);
            }
        }

        public PlaceableItem GetItem(Vector3 position)
        {
            PlaceableItem item;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
            }
            else
            {
                item = Instantiate(_prefab, transform);
            }

            item.transform.position = position;
            item.gameObject.SetActive(true);
            return item;
        }

        public void ReturnItem(PlaceableItem item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }
    }
}