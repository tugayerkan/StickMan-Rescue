using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SencanUtils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        var obj = new GameObject(typeof(T).ToString());
                        instance = obj.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
            
            if(instance == null)
                return;
        }
    }
}
