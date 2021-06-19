#if UNITY_EDITOR

using UnityEditor;

namespace SencanUtils.TrajectorySystem.Editor
{
    [CustomEditor(typeof(Trajectory))]
    public class TrajectoryEditor : UnityEditor.Editor
    {
        private SerializedProperty trajectoryType;
        private SerializedProperty positionCount;
        private SerializedProperty fireObjectPrefab;
        
        private SerializedProperty velocity;
        
        private SerializedProperty force;
        private SerializedProperty environmentTag;

        private void OnEnable()
        {
            trajectoryType = serializedObject.FindProperty("trajectoryType");
            positionCount = serializedObject.FindProperty("positionCount");
            fireObjectPrefab = serializedObject.FindProperty("fireObjectPrefab");

            velocity = serializedObject.FindProperty("velocity");

            force = serializedObject.FindProperty("force");
            environmentTag = serializedObject.FindProperty("environmentTag");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(trajectoryType);
            EditorGUILayout.PropertyField(positionCount);
            EditorGUILayout.PropertyField(fireObjectPrefab);

            EditorGUILayout.Space(10);
            
            if (trajectoryType.enumValueIndex == (int)Trajectory.Type.WithCollide)
            {
                EditorGUILayout.PropertyField(force);
                EditorGUILayout.PropertyField(environmentTag);
            }
            else
            {
                EditorGUILayout.PropertyField(velocity);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif
