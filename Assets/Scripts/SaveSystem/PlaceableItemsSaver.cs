using System;
using System.Collections.Generic;
using System.IO;
using Game.PlaceableItems;
using UnityEngine;

namespace SaveSystem
{
    public class PlaceableItemsSaver
    {
        private const string SavePath = "placed_items.json";
        private List<PlaceableItemData> _loadedItems = new();
        private bool _isInitialized = false;

        public void Initialize()
        {
            if (!_isInitialized)
            {
                LoadItems();
                _isInitialized = true;
            }
        }

        public void SaveItems(List<PlaceableItemData> items)
        {
            try
            {
                ItemsDataWrapper wrapper = new ItemsDataWrapper(items);
                string json = JsonUtility.ToJson(wrapper, true);
                string fullPath = Path.Combine(Application.persistentDataPath, SavePath);

                File.WriteAllText(fullPath, json);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void LoadItems()
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, SavePath);

                if (!File.Exists(fullPath))
                {
                    _loadedItems = new List<PlaceableItemData>();
                    return;
                }

                string json = File.ReadAllText(fullPath);
                ItemsDataWrapper wrapper = JsonUtility.FromJson<ItemsDataWrapper>(json);

                if (wrapper != null && wrapper.PlaceableItemDatas != null)
                {
                    _loadedItems = wrapper.PlaceableItemDatas;
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            _loadedItems = new List<PlaceableItemData>();
        }

        public List<PlaceableItemData> GetLoadedItems()
        {
            return new List<PlaceableItemData>(_loadedItems);
        }

        [Serializable]
        private class ItemsDataWrapper
        {
            public List<PlaceableItemData> PlaceableItemDatas;

            public ItemsDataWrapper(List<PlaceableItemData> placeableItemDatas)
            {
                PlaceableItemDatas = new List<PlaceableItemData>(placeableItemDatas);
            }
        }
    }
}