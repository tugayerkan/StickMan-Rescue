#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace SencanUtils.LevelManagement.Editor
{
    public class LevelManagementEditorWindow : EditorWindow
    {
        private ILevelCreator _levelManagerSo;
        
        private GUIStyle _titleStyle;
        private GUIStyle _fieldStyle;
        private GUIStyle _buttonStyle;

        private int _levelCount;
        private bool _hasLevelManager;
        
        public static void OpenWindow()
        {
            var window = (LevelManagementEditorWindow)GetWindow(typeof(LevelManagementEditorWindow), true, "Level Management");
            window.minSize = new Vector2(300, 225);
            window.maxSize = new Vector2(300, 225);
            window.Initialize();
            window.Show();
        }

        private void Initialize()
        {
            _hasLevelManager = Helper.LevelManagementHelper.HasLevelManagerSo();
            if (_hasLevelManager)
            {
                _levelManagerSo = Helper.LevelManagementHelper.GetLevelManagerSo() as ILevelCreator;
            }

            _titleStyle = new GUIStyle()
            {
                fontSize = 20,
                richText = true,
                fontStyle = FontStyle.BoldAndItalic,
                alignment = TextAnchor.UpperCenter
            };

            _fieldStyle = new GUIStyle()
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold
            };

            _buttonStyle = new GUIStyle("Button")
            {
                fontSize = !_hasLevelManager ? 15 : 20,
                richText = true,
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("<color=green>SU Level Management</color>", _titleStyle);
            EditorGUILayout.Space(50);

            _fieldStyle.padding.left = 15;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<color=white>Level Count:</color>", _fieldStyle, GUILayout.Width(125));
            _levelCount = Mathf.Clamp(_levelCount, 1, 100);
            _levelCount = EditorGUILayout.IntField(_levelCount, GUILayout.Width(125));
            EditorGUILayout.EndHorizontal();
            
            if (!_hasLevelManager)
            {
                EditorGUILayout.Space(50);
                if (GUILayout.Button("<color=yellow>Create Levels As Scene</color>", _buttonStyle, GUILayout.Height(35)))
                {
                    _levelManagerSo = Helper.LevelManagementHelper.CreateLevelManager<LevelManagerSoScene, SceneLevel>();
                    _levelManagerSo.CreateLevels(_levelCount);
                    _hasLevelManager = false;
                    Initialize();
                    Repaint();
                }

                if (GUILayout.Button("<color=darkblue>Create Levels As GameObject</color>", _buttonStyle, GUILayout.Height(35)))
                {
                    _levelManagerSo = Helper.LevelManagementHelper.CreateLevelManager<LevelManagerSoGameObject, GameObjectLevel>();
                    _levelManagerSo.CreateLevels(_levelCount);
                    _hasLevelManager = false;
                    Initialize();
                    Repaint();
                }
            }
            else
            {
                EditorGUILayout.Space(10);
                _fieldStyle.padding.left = 45;
                EditorGUILayout.LabelField($"<color=black>Current Level Count {_levelManagerSo.LevelCount}</color>", _fieldStyle, GUILayout.Width(100));
                EditorGUILayout.Space(40);
                
                if (_levelCount == _levelManagerSo.LevelCount)
                    GUI.enabled = false;
                
                if (GUILayout.Button($"<color={(_levelCount == _levelManagerSo.LevelCount ? "red" : "green")}>Change Level Count</color>", _buttonStyle, GUILayout.Height(50)))
                {
                    _levelManagerSo.RegenerateLevels(_levelCount);
                }
                GUI.enabled = true;
            }
        }
    }
}

#endif
