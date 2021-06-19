#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SencanUtils.LevelManagement.Editor
{
    [CustomEditor(typeof(LevelManagerSo<>), true)]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        private ILevelCreator _levelManager;

        private SerializedProperty _enumProp;
        private SerializedProperty _minValueProp;
        private SerializedProperty _maxValueProp;

        protected virtual void OnEnable()
        {
            _levelManager = (ILevelCreator)target;

            _enumProp = serializedObject.FindProperty("randomTypeAfterLevelsFinished");
            _minValueProp = serializedObject.FindProperty("minRandomValue");
            _maxValueProp = serializedObject.FindProperty("maxRandomValue");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_enumProp);
            if (_enumProp.enumValueIndex == 1)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(_minValueProp);
                EditorGUILayout.PropertyField(_maxValueProp);
            }
            else
            {
                _minValueProp.intValue = 0;
                _maxValueProp.intValue = _levelManager.LevelCount;
            }
            
            _maxValueProp.intValue = Mathf.Clamp(_maxValueProp.intValue, _minValueProp.intValue + 1, _levelManager.LevelCount);
            _minValueProp.intValue = Mathf.Clamp(_minValueProp.intValue, 0, _maxValueProp.intValue - 1);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
