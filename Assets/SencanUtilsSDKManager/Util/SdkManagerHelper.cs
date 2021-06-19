using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace SencanUtilsSDKManager.Util
{
    public static class SdkManagerHelper
    {
        public const string AssetPathPrefix = "Assets/";
        public const string DownloadDirectory = AssetPathPrefix + "SencanUtilsSDKManager/";
        
        public static bool IsEqualVersions(string version1, string version2)
        {
            return version1.Equals(version2);
        }

        public static int CompareVersions(string version1, string version2)
        {
            if (string.IsNullOrEmpty(version1) || string.IsNullOrEmpty(version2)) return -1;
            
            var versionA = VersionStringToIntegers(version1);
            var versionB = VersionStringToIntegers(version2);
            for (var i = 0; i < Mathf.Max(versionA.Length, versionB.Length); i++)
            {
                if (VersionPiece(versionA, i) < VersionPiece(versionB, i))
                    return -1;
                if (VersionPiece(versionA, i) > VersionPiece(versionB, i))
                    return 1;
            }

            return 0;
        }
        
        private static int VersionPiece(IList<int> versionInts, int pieceIndex)
        {
            return pieceIndex < versionInts.Count ? versionInts[pieceIndex] : 0;
        }

        private static int[] VersionStringToIntegers(string version)
        {
            int piece;
            return version.Split('.')
                .Select(str => int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out piece) ? piece : 0)
                .ToArray();
        }
    }
}
