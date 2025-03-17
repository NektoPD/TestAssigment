using System;
using UnityEngine;

namespace Game.PlaceableItems
{
    [Serializable]
    public class PlaceableItemData
    {
        public Vector3 Position;
        public ItemType ItemType;
    }
}