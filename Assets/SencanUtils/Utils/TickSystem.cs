using System;
using UnityEngine;

namespace SencanUtils
{
    public static class TickSystem
    {
        public static event Action<int> onTick;

        private const float TICK_TIMER = 0.25f;
        public static float TickTimer => TICK_TIMER;

        private static int tickCount;
        public static int TickCount => tickCount;

        private static GameObject tickSystemObject;
        
        public static void Init()
        {
            if (tickSystemObject != null) 
                return;
            
            tickSystemObject = new GameObject("TickSystem");
            tickSystemObject.AddComponent<TickSystemMonoBehavior>();
        }

        private class TickSystemMonoBehavior : MonoBehaviour
        {
            private float tickCounter = 0;
            private void Update()
            {
                tickCounter += Time.deltaTime;
                if (tickCounter >= TICK_TIMER)
                {
                    ++tickCount;
                    onTick?.Invoke(tickCount);
                    tickCounter = 0;
                }
            }

            private void OnDestroy()
            {
                tickSystemObject = null;
            }
        }
    }
}