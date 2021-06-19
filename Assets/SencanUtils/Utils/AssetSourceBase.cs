using UnityEngine;

namespace SencanUtils.Utils
{
    public class AssetSourceBase : ScriptableObject
    {
        protected static T GetOrLoad<T>(ref T instance) where T : AssetSourceBase
        {
            if (instance == null)
            {
                var name = typeof(T).Name;
                instance = Resources.Load<T>(name);
                
                if (instance == null)
                    Debug.LogWarning($"Failed to load index: '{name}'.\nIndex file must be placed at: Resources/{name}.asset");
            }

            return instance;
        }
    }
}
