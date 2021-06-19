using UnityEngine;

namespace SUSDKTools
{
    public static class SDKToolHelper
    {
        public static bool IsAvailablePlatform()
        {
            return !Application.isEditor;
        }
    }
}
