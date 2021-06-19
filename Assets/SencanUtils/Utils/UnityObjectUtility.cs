using UnityObject = UnityEngine.Object;

namespace SencanUtils.Utils
{
    public static class UnityObjectUtility
    {
        public static bool IsDestroyed(this UnityObject target)
        {
            return !ReferenceEquals(target, null) && target == null;
        }
        
        public static bool IsUnityNull(this object obj)
        {
            return obj == null || obj is UnityObject uo && uo == null;
        }
        
        public static T AsUnityNull<T>(this T obj) where T : UnityObject
        {
            return obj == null ? null : obj;
        }
    }
}
