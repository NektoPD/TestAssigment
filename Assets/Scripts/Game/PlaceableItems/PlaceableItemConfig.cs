using UnityEngine;

namespace Game.PlaceableItems
{
    [CreateAssetMenu(fileName = "New Placeable Item", menuName = "Placeable Items/Create new item")]
    public class PlaceableItemConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ItemType Type { get; private set; }
    }
}