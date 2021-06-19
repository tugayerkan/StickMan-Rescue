using DG.Tweening;
using UnityEngine;

namespace SencanUtils
{
    public class Pulse : MonoBehaviour, IAnimatable
    {
        private Tween tween;
        private bool isOverButton;
        
        public void OnDown(DoButton doButton)
        {
            tween.Kill();
            tween = doButton.transform.DOLocalRotate(new Vector3(0, 0, 180f), .2f);
        }

        public void OnUp(DoButton doButton)
        {
           tween.Kill();
           tween = doButton.transform.DOLocalRotate(Vector3.zero, 0.2f);
        }
    }
}