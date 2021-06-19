using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace MU.MergeMechanic.Scripts
{
    public class CreateItem : MonoBehaviour
    {
        [SerializeField] private bool autoMerge;
        [Tooltip("Merge Delay Type is Seconds!")]
        [Range(0, 5f)]
        [SerializeField] private float mergeDelay = 1f;

        /*[HideInInspector]*/ public List<GameObject> _currentItems = new List<GameObject>();
        /*[HideInInspector]*/ public List<ItemData> _items = new List<ItemData>();
        private LevelInfo _levelInfo;
        private bool _canMerge;

        private void Awake()
        {
            _levelInfo = FindObjectOfType<LevelInfo>();
        }
        
        public bool CheckMerge(ItemData data)
        {
            if (_items.Count > 1)
            {
                for (var x = 0; x < _items.Count; x++)
                {
                    if (data.createItemData == _items[x].createItemData)
                    {
                        _canMerge = true;
                    }
                    else
                    {
                        _canMerge = false;
                    }
                }
            }

            else
            {
                _canMerge = true;
            }

            return _canMerge;
        }

        public void CreateHere(Vector3 pos, Quaternion rot, ItemData data)
        {
            var go = Instantiate(data.itemPrefab, pos, rot);
            _currentItems.Add(go);
            _items.Add(data);

            if (_items.Count > 1)
            {
                for (var x = 0; x < _items.Count; x++)
                {
                    if (_items[x].createItemData == data.createItemData)
                    {
                        CanMerge(go, _currentItems[x]);
                    }
                }
            }
        }

        private async void CanMerge(GameObject _first, GameObject _second)
        {
            if (autoMerge)
            {
                await Task.Delay(TimeSpan.FromSeconds(mergeDelay));
                
                _currentItems.Remove(_first);
                Destroy(_first);
                _currentItems.Remove(_second);
                Destroy(_second);
            }
            else
            {
                //TODO
                //_first.AddComponent<QuickOutline>().OutlineColor = Color.white;
                _first.GetComponent<DragDrop>().canMerge = true;
                //_second.AddComponent<QuickOutline>().OutlineColor = Color.white;
                _second.GetComponent<DragDrop>().canMerge = true;
            }
        }
    }
}