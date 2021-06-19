using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MU.TimingMechanic.Scripts
{
    public class StartTimer : MonoBehaviour
    {
        [SerializeField] private Image countImage;
        [SerializeField] private List<Sprite> sprites;
        
        //public void StartButton(Button thisButton)
        //{
        //    if (countImage == null) return;
        //    Timer.StartCountDown(countImage, sprites);
        //    thisButton.gameObject.SetActive(false);
        //}
    }
}
