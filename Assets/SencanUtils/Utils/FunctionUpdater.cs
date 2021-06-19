using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace SencanUtils
{
    public class FunctionUpdater
    {
        private static List<FunctionUpdater> updaterList;

        private static bool InitIfNeeded()
        {
            if (updaterList == null)
            {
                updaterList = new List<FunctionUpdater>();
                return true;
            }

            return false;
        }

        public static FunctionUpdater Create(Action updateFunc)
        {
            return Create(() => { updateFunc(); return false; });
        }

        public static FunctionUpdater Create(Action updateFunc, string functionName)
        {
            return Create(() => { updateFunc(); return false; }, functionName);
        }

        public static FunctionUpdater Create(Action updateFunc, string functionName, bool active)
        {
            return Create(() => { updateFunc(); return false; }, functionName, active);
        }
        
        public static FunctionUpdater Create(Action updateFunc, string functionName, bool active, bool stopAllWithSameName)
        {
            return Create(() => { updateFunc(); return false; }, functionName, active, stopAllWithSameName);
        }
        
        public static FunctionUpdater Create(Func<bool> updateFunc)
        {
            return Create(updateFunc, string.Empty, true, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName)
        {
            return Create(updateFunc, functionName, true, false);
        }
        
        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active)
        {
            return Create(updateFunc, functionName, active, false);
        }

        public static FunctionUpdater Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName)
        {
            InitIfNeeded();

            if (stopAllWithSameName)
                StopAllUpdatersWithName(functionName);
            
            FunctionUpdater functionUpdater = new FunctionUpdater(updateFunc, functionName, active);
            
            updaterList.Add(functionUpdater);
            return functionUpdater;
        }

        private static void RemoveUpdater(FunctionUpdater funcUpdater)
        {
            if(InitIfNeeded())
                return;
            updaterList.Remove(funcUpdater);
        }

        public static void DestroyUpdater(FunctionUpdater funcUpdater)
        {
            if(InitIfNeeded())
                return;
            funcUpdater?.DestroySelf();
        }

        public static void DestroyAllUpdater()
        {
            for (var i = 0; i < updaterList.Count; i++)
            {
                updaterList[i].DestroySelf();
            }
        }

        public static void StopUpdaterWithName(string functionName)
        {
            if(InitIfNeeded())
                return;
            for (var i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    return;
                }
            }
        }

        public static void StopAllUpdatersWithName(string functionName)
        {
            if(InitIfNeeded())
                return;
            for (var i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    i--;
                }
            }
        }
        
        private string functionName;
        private bool active;
        private bool isDestroyed;
        private Func<bool> updateFunc; // Destroy Updater if return true;

        private FunctionUpdater(Func<bool> updateFunc, string functionName, bool active)
        {
            this.updateFunc = updateFunc;
            this.functionName = functionName;
            this.active = active;
            isDestroyed = false;

            #pragma warning disable
            Update();
        }

        public void Pause()
        {
            active = false;
        }

        public void Resume()
        {
            active = true;
        }

        private async UniTask Update()
        {
            while (!isDestroyed)
            {
                if (!active)
                    await UniTask.Yield();
                else
                {
                    await UniTask.Yield();
                    if (updateFunc())
                        DestroySelf();
                }
            }
        }

        private void DestroySelf()
        {
            isDestroyed = true;
            RemoveUpdater(this);
        }

    }
}
