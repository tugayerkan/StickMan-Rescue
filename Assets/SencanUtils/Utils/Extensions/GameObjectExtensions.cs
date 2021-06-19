using UnityEngine;

namespace SencanUtils.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject GetChildWithTag(this GameObject parent, string tag)
        {
            GameObject result = null;
            GetObject(parent.transform, tag, ref result);
            return result;
        }

        private static void GetObject(Transform parent, string tag, ref GameObject result)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag))
                {
                    result = child.gameObject;
                    return;
                }
                if (child.childCount > 0)
                    GetObject(child, tag, ref result);
            }
        }
    }
}