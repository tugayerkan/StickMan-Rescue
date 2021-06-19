#if UNITY_EDITOR

using SencanUtils.LevelManagement.Editor;
using SencanUtils.SaveUtils;
using SencanUtils.UI.Shop.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace SencanUtils.Editor
{
    public static class SUWindowManager
    {
        [MenuItem("Utils/SencanUtils/Level Management Window", false, 0)]
        private static void OpenLevelManagementEditor()
        {
            LevelManagementEditorWindow.OpenWindow();
        }
        
        [MenuItem("Utils/SencanUtils/Mesh Simplifier", false, 1)]
        private static void OpenMeshSimplifierWindow()
        {
            MeshSimplifierWindow.OpenWindow();
        }

        [MenuItem("Utils/SencanUtils/PlayerPrefs Editor", false, 2)]
        public static void OpenPlayerPrefsWindow()
        {
            KeyEditorWindow.OpenWindow();
        }
        
        [MenuItem("Utils/SencanUtils/Reset PlayerPrefs", false, 3)]
        private static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Utils/SencanUtils/Delete All Saved Data", false, 4)]
        private static void DeleteAllSavedData()
        {
            SaveSystem.DeleteAllData();
        }

        [MenuItem("Utils/SencanUtils/Shop/ItemEditor", false, 5)]
        private static void ItemEditor()
        {
            ShopEditor.OpenWindow();
        }
    }
}

#endif
