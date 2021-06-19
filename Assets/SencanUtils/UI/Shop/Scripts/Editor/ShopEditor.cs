#if UNITY_EDITOR

using System.Collections.Generic;
using SencanUtils.SaveUtils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SencanUtils.UI.Shop.Scripts.Editor
{
    public class ShopEditor : EditorWindow
    {
        private ScrollView _scrollView;
        private ItemSo _itemSo;
        
        public static void OpenWindow()
        {
            var window = GetWindow<ShopEditor>(true);
            window.titleContent = new GUIContent("Item Editor");
        }

        private void OnEnable()
        {
            var root = rootVisualElement;
            root.style.flexDirection = FlexDirection.Row;

            var itemsListBox = new Box();
            itemsListBox.style.flexGrow = 1f;
            itemsListBox.style.flexShrink = 0f;
            itemsListBox.style.flexBasis = 0f;
            itemsListBox.style.flexDirection = FlexDirection.Column;

            var newItemBox = new Box();
            newItemBox.style.flexGrow = 3f;
            newItemBox.style.flexShrink = 0f;
            newItemBox.style.flexBasis = 0f;

            SetupFields(newItemBox);
            SetupItemList(itemsListBox);

            root.Add(itemsListBox);
            root.Add(newItemBox);
            
            LoadItems();
        }

        private void SetupFields(VisualElement parent)
        {
            var newItemLabel = new Label("New Item");
            newItemLabel.style.alignSelf = Align.Center;
            newItemLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            parent.Add(newItemLabel);

            var nameField = new TextField("Name: ");
            var priceField = new IntegerField("Price: ");
            var itemUI = new ObjectField("Item UI: ") {objectType = typeof(Sprite)};

            parent.Add(nameField);
            parent.Add(priceField);
            parent.Add(itemUI);

            var saveItemButton = new Button
            {
                text = "Save Item"
            };
            saveItemButton.clicked += () =>
            {
                if(!string.IsNullOrEmpty(nameField.value) && priceField.value > 0)
                    SaveItem(new ItemSo.ItemData()
                    {
                        itemName = nameField.value,
                        itemUI = (Sprite)itemUI.value,
                        itemPrice = priceField.value
                    });
            };

            parent.Add(saveItemButton);
        }

        private void SetupItemList(VisualElement parent)
        {
            var listLabel = new Label("Item List");
            listLabel.style.alignSelf = Align.Center;
            listLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            parent.Add(listLabel);

            _scrollView = new ScrollView
            {
                showHorizontal = false
            };
            _scrollView.style.flexGrow = 1f;
            parent.Add(_scrollView);
        }

        private void CreateListItem(ItemSo.ItemData itemSo)
        {
            var itemElement = new VisualElement();
            itemElement.style.flexDirection = FlexDirection.Row;
            itemElement.focusable = true;

            var remove = new Button {text = "-"};
            remove.clicked += () =>
            {
                _scrollView.contentContainer.Remove(itemElement);
                _itemSo.RemoveItem(itemSo);
            };
            itemElement.Add(remove);

            var nameButton = new Button
            {
                text = itemSo.itemName
            };
            nameButton.style.flexGrow = 1f;
            itemElement.Add(nameButton);

            _scrollView.contentContainer.Add(itemElement);
        }

        private void SaveItem(ItemSo.ItemData itemData)
        {
            _itemSo.AddItem(itemData);
            CreateListItem(itemData);
        }

        private void LoadItems()
        {
            _itemSo = Resources.Load<ItemSo>("ItemData/Items");
            
            foreach (var savedItem in _itemSo.ItemDataList)
            {
                CreateListItem(savedItem);
            }
        }
    }
}

#endif