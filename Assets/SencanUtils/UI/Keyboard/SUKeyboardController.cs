using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SencanUtils.UI.Keyboard
{
    public class SUKeyboardController : MonoBehaviour
    {
        #region Singleton
        private static SUKeyboardController instance;
        public static SUKeyboardController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SUKeyboardController>();
                    if (instance == null)
                        instance = Resources.Load<GameObject>("SUKeyboard").GetComponent<SUKeyboardController>();
                }

                return instance;
            }
        }
        #endregion
        
        public static event Action<string> onEnterPressed;
        public static event Action<string> onButtonPressed; 

        private bool isEnable;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                if (value != isEnable)
                {
                    if(value)
                        EnableKeyboard();
                    else
                        DisableKeyboard();
                }
                isEnable = value;
            }
        }

        [SerializeField] private RectTransform lettersParent = default;
        [SerializeField] private float enablePosY = 7.5f;
        [SerializeField] private float disablePosY = -960f;
        [SerializeField] private float moveTime = 1f;
        
        
        private Text textBox;
        private string currentText;

        private string CurrentText
        {
            get => currentText;
            set
            {
                currentText = value;
                textBox.text = currentText;
            }
        }

        private void Awake()
        {
            textBox = GameObject.FindWithTag("SencanUtils/Keyboard/Text").GetComponent<Text>();
            CurrentText = string.Empty;
        }

        public void ResetText()
        {
            CurrentText = string.Empty;
        }

        public void OnButtonPressed(string letter)
        {
            if (isEnable)
            {
                CurrentText += letter;
                onEnterPressed?.Invoke(currentText);
            }
        }

        public void OnEnterPressed()
        {
            if (isEnable && !string.IsNullOrEmpty(CurrentText))
            {
                onEnterPressed?.Invoke(CurrentText);
                CurrentText = string.Empty;
            }
        }

        public void OnBackspacePressed()
        {
            if (!string.IsNullOrEmpty(CurrentText))
            {
                CurrentText = CurrentText.Remove(CurrentText.Length - 1, 1);
                onButtonPressed?.Invoke(CurrentText);
            }
        }

        private void EnableKeyboard()
        {
            var pos = lettersParent.anchoredPosition;
            DOTween.To(() => pos.y, y => pos.y = y, enablePosY, moveTime)
                .OnUpdate(() => lettersParent.anchoredPosition = pos);
        }

        private void DisableKeyboard()
        {
            var pos = lettersParent.anchoredPosition;
            DOTween.To(() => pos.y, y => pos.y = y, disablePosY, moveTime)
                .OnUpdate(() => lettersParent.anchoredPosition = pos);
        }
    }
}
