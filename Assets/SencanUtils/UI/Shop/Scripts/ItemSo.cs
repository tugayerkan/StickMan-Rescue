using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SencanUtils.UI.Shop.Scripts
{
    [CreateAssetMenu(menuName = "SencanUtils/UI/Shop/Item Data")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private List<ItemData> itemDataList = new List<ItemData>(StoreManager.ITEM_COUNT);
        public ReadOnlyCollection<ItemData> ItemDataList => itemDataList.AsReadOnly();

        public bool AddItem(ItemData item)
        {
            if(itemDataList.Count >= StoreManager.ITEM_COUNT)
                return false;

            itemDataList.Add(item);
            return true;
        }

        public bool RemoveItem(ItemData item)
        {
            return itemDataList.Remove(item);
        }
        
        private void OnValidate()
        {
            if(itemDataList.Count >= StoreManager.ITEM_COUNT)
                itemDataList.RemoveRange(StoreManager.ITEM_COUNT, itemDataList.Count - StoreManager.ITEM_COUNT);
        }

        [Serializable]
        public struct ItemData
        {
            public string itemName;
            public Sprite itemUI;
            [Min(0)] public int itemPrice;
        }
    }
}