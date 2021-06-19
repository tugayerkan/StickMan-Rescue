using System;
using System.Security.Cryptography;
using MusaUtils;
using UnityEngine;

namespace MU.MergeMechanic.Scripts
{
    public class DragDrop : MonoBehaviour
    {
        [SerializeField] private ItemData _data;
        [HideInInspector] public bool canMerge;
        private Camera _camera;
        private Vector3 _firstPos;
        private CreateItem _createItem;
        
        private void Awake()
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
            _createItem = FindObjectOfType<CreateItem>();
            _camera = Camera.main;
            _firstPos = transform.position;
        }

        private void OnMouseDrag()
        {
            transform.position = Vector3.Slerp(transform.position, QuickRay.Point(_camera), .15f);
        }

        private void OnMouseUp()
        {
            if (!canMerge)
            {
                if (_createItem.CheckMerge(_data))
                {
                    _createItem.CreateHere(transform.position, transform.rotation, _data);
                    Destroy(gameObject);
                }
                else
                {
                    Vector3.Slerp(transform.position, _firstPos, .25f);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canMerge)
            {
                if (other.GetComponent<DragDrop>()._data.createItemData == _data.createItemData)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    Instantiate(_data.itemPrefab, transform.position, transform.rotation);
                }
            }
        }
    }
}
