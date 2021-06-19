using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SencanUtils
{
    public static class FunctionPeriodic
    {
        private static List<FunctionPeriodicSystem> periodicSystemList;
        private static int Id;
        
        public static int Create(Action action, float periodicTime, string name = null)
        {
            InitIfNeeded();
            
            var periodicTimer = new FunctionPeriodicSystem(Id, action, periodicTime, name);
            periodicSystemList.Add(periodicTimer);
            
            return Id++;
        }
        
        public static void Remove(int id)
        {
            InitIfNeeded();
            
            var obj = periodicSystemList.Find(I => I.Id == id);
            if (obj != null)
            {
                obj.isDestroyed = true;
                periodicSystemList.Remove(obj);
            }
        }
        
        public static void Remove(string name)
        {
            InitIfNeeded();
            
            var obj = periodicSystemList.Find(I => I.name.Equals(name));
            if (obj != null)
            {
                obj.isDestroyed = true;
                periodicSystemList.Remove(obj);
            }
        }

        private static void InitIfNeeded()
        {
            if (periodicSystemList != null) return;
            
            Id = 0;
            periodicSystemList = new List<FunctionPeriodicSystem>();
        }
        
        private class FunctionPeriodicSystem
        {
            public int Id;
            public string name;
            public bool isDestroyed;
            
            private Action action;
            private float periodicTime;
            private GameObject handler;

            public FunctionPeriodicSystem(int Id, Action action, float periodicTime, string name = null)
            {
                this.Id = Id;
                this.action = action;
                this.periodicTime = periodicTime;
                this.name = name;

                #pragma warning disable
                Update();
            }

            private async void Update()
            {
                while (!isDestroyed)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(periodicTime));
                    action();
                }
            }
        }
    }
}
