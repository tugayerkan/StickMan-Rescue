#if UNITY_EDITOR

using UnityEditor;

namespace SencanUtils.LevelManagement.Editor
{
    [CustomEditor(typeof(LevelManagerSoScene))]
    public class LevelManagerSceneEditor : LevelManagerEditor
    {
        private SerializedProperty _showLoadingScreen;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _showLoadingScreen = serializedObject.FindProperty("showLoadingScreen");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(_showLoadingScreen);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif