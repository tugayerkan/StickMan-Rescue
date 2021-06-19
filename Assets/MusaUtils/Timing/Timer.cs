using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MU
{
    public static class Timer
    {
        private static Color _aColor = new Color(0, 1, 0);
        private static Color _bColor = new Color(1, 0, 0);

        public static async void StartCountDown(Text countImage, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (i < 5)
                {
                    SetAlpha(countImage, false);
                }
                else
                {
                    SetAlpha(countImage, true);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            }


            if (UniTask.CompletedTask.AsTask().IsCompleted)
            {
                countImage.gameObject.SetActive(false);
            }
        }

        public static async void SetAlpha(Text image, bool isRed)
        {
            for (float i = .85f; i >= 0f; i -= .015f)
            {
                if (!isRed)
                {
                    _aColor.a = i;

                    image.color = _aColor;
                       

                }
                else
                {
                    if (image != null)
                    {

                        _bColor.a = i;
                        image.color = _bColor;


                    }
                }
                await UniTask.Delay(TimeSpan.FromMilliseconds(5));
            }
        }
    }
}
