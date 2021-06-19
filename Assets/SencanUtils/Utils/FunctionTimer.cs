using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace SencanUtils
{
    public class FunctionTimer
    {
        public static FunctionTimer Create(Action action, float timer, string name = null)
        {
            InitIfNeeded();
            
            FunctionTimer functionTimer = new FunctionTimer(action, timer, name);
            
            #pragma warning disable
            functionTimer.Execute();
            
            activeTimerList.Add(functionTimer);

            return functionTimer;
        }

        private async UniTask Execute()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timer), cancellationToken: _cancellationToken.Token);
            action();
            DestroySelf();
        }
        
        public static bool StopTimer(FunctionTimer functionTimer)
        {
            InitIfNeeded();
            FunctionTimer func = activeTimerList.Find(I => I == functionTimer);
            if (func != null)
            {
                func._cancellationToken.Cancel();
                func.DestroySelf();
                return true;
            }

            return false;
        }

        public static bool StopTimer(string name)
        {
            InitIfNeeded();
            for (var i = 0; i < activeTimerList.Count; i++)
            {
                if (activeTimerList[i].name == name)
                {
                    activeTimerList[i]._cancellationToken.Cancel();
                    activeTimerList[i].DestroySelf();
                    return true;
                }
            }

            return false;
        }
        
        public static void StopAllTimers()
        {
            if(InitIfNeeded())
                return;
            
            for (int i = 0; i < activeTimerList.Count; i++)
            {
                StopTimer(activeTimerList[i]);
            }
            activeTimerList.Clear();
        }

        private static bool InitIfNeeded()
        {
            if (activeTimerList == null)
            {
                activeTimerList = new List<FunctionTimer>();
                return true;
            }

            return false;
        }

        private static void Remove(FunctionTimer functionTimer)
        {
            InitIfNeeded();
            activeTimerList.Remove(functionTimer);
        }

        private static List<FunctionTimer> activeTimerList;

        private readonly Action action;
        private float _timer;
        private readonly string name;
        private readonly CancellationTokenSource _cancellationToken;

        private FunctionTimer(Action action, float timer, string name = null)
        {
            this.action = action;
            this._timer = timer;
            this.name = name;
            _cancellationToken = new CancellationTokenSource();
        }

        private void DestroySelf()
        {
            Remove(this);
        }
    }
}
