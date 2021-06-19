using System.Collections.Generic;
using UnityEngine;

namespace SencanUtils
{
    public static class SuWaitForSeconds
    {
        private const int POOL_SIZE = 4;

        private static readonly Stack<WaitForSeconds> waitForSecondsPool;
        private static readonly Stack<WaitForSecondsRealtime> waitForSecondsRealtimePool;

        static SuWaitForSeconds()
        {
            waitForSecondsPool = new Stack<WaitForSeconds>(POOL_SIZE);
            waitForSecondsRealtimePool = new Stack<WaitForSecondsRealtime>(POOL_SIZE);
            
            for( int i = 0; i < POOL_SIZE; i++ )
            {
                waitForSecondsPool.Push( new WaitForSeconds());
                waitForSecondsRealtimePool.Push(new WaitForSecondsRealtime());
            }
        }
        
        public static CustomYieldInstruction Wait( float seconds )
        {
            WaitForSeconds instance;
            if( waitForSecondsPool.Count > 0 )
                instance = waitForSecondsPool.Pop();
            else
                instance = new WaitForSeconds();

            instance.Initialize( seconds );
            return instance;
        }

        public static CustomYieldInstruction WaitRealtime( float seconds )
        {
            WaitForSecondsRealtime instance;
            if( waitForSecondsRealtimePool.Count > 0 )
                instance = waitForSecondsRealtimePool.Pop();
            else
                instance = new WaitForSecondsRealtime();

            instance.Initialize( seconds );
            return instance;
        }
        
        private static void Pool( WaitForSeconds instance )
        {
            waitForSecondsPool.Push( instance );
        }

        private static void Pool( WaitForSecondsRealtime instance )
        {
            waitForSecondsRealtimePool.Push( instance );
        }
        
        private class WaitForSeconds : CustomYieldInstruction
        {
            public override bool keepWaiting
            {
                get
                {
                    if (Time.time < waitUntil)
                        return true;

                    Pool(this);
                    return false;
                }
            }
            
            private float waitUntil;

            public void Initialize(float seconds)
            {
                waitUntil = Time.time + seconds;
            }
        }
        
        private class WaitForSecondsRealtime : CustomYieldInstruction
        {
            public override bool keepWaiting
            {
                get
                {
                    if (Time.realtimeSinceStartup < waitUntil)
                        return true;

                    Pool(this);
                    return false;
                }
            }

            private float waitUntil;
            
            public void Initialize(float seconds)
            {
                waitUntil = Time.realtimeSinceStartup + seconds;
            }
        }
    }
}
