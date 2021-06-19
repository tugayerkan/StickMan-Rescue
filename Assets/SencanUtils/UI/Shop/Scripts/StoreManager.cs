using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using SencanUtils.SaveUtils;
using SencanUtils.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SencanUtils.UI.Shop.Scripts
{
    public class StoreManager : MonoBehaviour
    {
        public const int ITEM_COUNT = 10;

        public static event Action onHomeButtonPressed; 
        public static event Action<string> onItemSelected;

        [SerializeField] private ItemSo itemSo = default;
        [SerializeField] private Button unlockButton = default;
        
        private Item[] items;
        private Item[] unLockedItems;
        private int money;

        private bool _isUnlocking;

        private void Awake()
        {
            SaveSystem.Save("Money", 1000, SaveSystem.SaveType.PlayerPrefs);
            money = SaveSystem.Load<int>("Money", SaveSystem.SaveType.PlayerPrefs);

            if (itemSo == null)
                itemSo = Resources.Load<ItemSo>("ItemData/Items");

            InitializeItems();
        }

        private void OnEnable()
        {
            unLockedItems = items.Where(I => !I.IsOpen).ToArray();
            if (unLockedItems.Length == 0)
                unlockButton.interactable = false;
        }

        private void InitializeItems()
        {
            items = FindObjectsOfType<Item>();
            Array.ForEach(items, I => I.Initialize());
            Array.Sort(items, (I, x) => I.Price.CompareTo(x.Index));
            for (var i = 0; i < items.Length; i++)
            {
                items[i].ItemName = itemSo.ItemDataList[i].itemName;
                items[i].Price = itemSo.ItemDataList[i].itemPrice;
                items[i].Image = itemSo.ItemDataList[i].itemUI;
            }
        }
        private async void UnlockRandomItem(float timeToSelect, float delayPerItem)
        {
            _isUnlocking = true;
            float time = 0f;
            Item randItem = null;
            Item prevRandItem = null;
            while (timeToSelect > time)
            {
                unLockedItems = items.Where(I => !I.IsOpen).ToArray();
                if (unLockedItems.Length == 1)
                {
                    randItem = unLockedItems[0];
                    break;
                }
                
                randItem = unLockedItems.GetRandom();
                while (prevRandItem == randItem)
                    randItem = unLockedItems.GetRandom();

                randItem.IsHighLighted = true;
                await UniTask.Delay(TimeSpan.FromSeconds(delayPerItem));
                randItem.IsHighLighted = false;
                prevRandItem = randItem;
                time += delayPerItem;
            }

            if (randItem == null) return;
            randItem.IsHighLighted = true;
            FunctionTimer.Create(() =>
            {
                randItem.Open();
                onItemSelected?.Invoke(randItem.ItemName);
                _isUnlocking = false;
            }, 0.5f);
        } 

        public void OnUnlockRandomPressed()
        {
            if(!_isUnlocking)
                 UnlockRandomItem(Random.Range(5f, 10f), Random.Range(0.3f, 0.5f));
        }

        public void OnHomeButtonPressed()
        {
            onHomeButtonPressed?.Invoke();
        }
    }
}
