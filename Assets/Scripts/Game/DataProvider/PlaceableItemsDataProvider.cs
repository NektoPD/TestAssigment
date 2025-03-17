using System.Linq;
using Game.PlaceableItems;
using UnityEngine;

namespace Game.DataProvider
{
    public class PlaceableItemsDataProvider : MonoBehaviour
    {
        [SerializeField] private PlaceableItemConfig[] _placeableItemConfigs;

        public PlaceableItemConfig GetDataByType(ItemType type)
        {
            return _placeableItemConfigs.FirstOrDefault(placeableItemConfig => placeableItemConfig.Type == type);
        }
    }
}