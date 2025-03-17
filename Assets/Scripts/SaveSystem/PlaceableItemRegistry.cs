using System.Collections.Generic;
using Game.PlaceableItems;

namespace SaveSystem
{
    public class PlaceableItemRegistry
    {
        private readonly List<PlaceableItem> _activeItems = new List<PlaceableItem>();

        public void RegisterItem(PlaceableItem item)
        {
            if (item != null && !_activeItems.Contains(item))
            {
                _activeItems.Add(item);
            }
        }

        public void UnregisterItem(PlaceableItem item)
        {
            if (item != null)
            {
                _activeItems.Remove(item);
            }
        }

        public List<PlaceableItemData> GetAllItemData()
        {
            List<PlaceableItemData> itemsData = new List<PlaceableItemData>();

            foreach (var item in _activeItems)
            {
                if (!item.IsPlaceable)
                {
                    itemsData.Add(item.GetSaveData());
                }
            }

            return itemsData;
        }
    }
}