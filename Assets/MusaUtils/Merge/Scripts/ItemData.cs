using JetBrains.Annotations;
using UnityEngine;

namespace MU.MergeMechanic.Scripts
{
    [CreateAssetMenu(menuName = "MU/MergeItem", fileName = "MergeItem")]
    public class ItemData : ScriptableObject
    {
        public int index;
        [CanBeNull] public string itemName;
        public int itemLevel;
        [NotNull]
        public GameObject itemPrefab;
        public ItemData createItemData;
    }
}
