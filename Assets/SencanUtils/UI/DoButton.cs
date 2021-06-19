using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SencanUtils
{
    public class DoButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
    {
        public UnityEvent onClick;
        
        public Image images;
        private IAnimatable animatable;

        public IAnimatable Animatable
        {
            get
            {
                if (animatable != null) return animatable;
                animatable = GetComponent<IAnimatable>();
                return animatable;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            animatable.OnDown(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            animatable.OnUp(this);
        }

        private void OnClick()
        {
            
            
            onClick?.Invoke();
        }
    }
}
