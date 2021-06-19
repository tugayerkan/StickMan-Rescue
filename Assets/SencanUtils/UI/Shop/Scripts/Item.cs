using SencanUtils.SaveUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SencanUtils.UI.Shop.Scripts
{
    public class Item : MonoBehaviour
    {
        private Image image;
        private Image lockImage;
        private GameObject highLight;
        private Sprite itemImage;
        
        private bool isHighLighted;

        public string ItemName { get; set; }

        public bool IsOpen
        {
            get => SaveSystem.Load<bool>(Index.ToString(), SaveSystem.SaveType.PlayerPrefs);
            private set
            {
                SaveSystem.Save(Index.ToString(), value, SaveSystem.SaveType.PlayerPrefs);
                if (!value)
                    IsHighLighted = false;
            }
        }

        public bool IsHighLighted
        {
            get => isHighLighted;
            set
            {
                isHighLighted = value;
                highLight.SetActive(isHighLighted);
            }
        }
        
        public Sprite Image
        {
            get => itemImage;
            set
            {
                itemImage = value;
                image.sprite = itemImage;
            }
        }

        public int Index { get; set; }
        public int Price { get; set; }

        public void Initialize()
        {
            Index = int.Parse(gameObject.name);
            image = transform.GetChild(0).GetComponent<Image>();
            lockImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            highLight = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            IsHighLighted = false;
            
            if(IsOpen)
                lockImage.gameObject.SetActive(false);
        }

        public void Open()
        {
            IsHighLighted = false;
            IsOpen = true;
            lockImage.CrossFadeAlpha(0f, 0.5f, true);
        }
    }
}
