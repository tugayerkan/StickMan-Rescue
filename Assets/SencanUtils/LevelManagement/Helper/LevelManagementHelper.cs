#if UNITY_EDITOR
    using UnityEditor;
#endif
    
    using System.IO;
    using System.Linq;
    using UnityEngine;

namespace SencanUtils.LevelManagement.Helper
{
    public static class LevelManagementHelper
    {
        public static readonly string PrefixPath = "Assets/Levels/";
        public static readonly string ScenePath = PrefixPath + "Scenes/";
        public static readonly string LevelDataPath = PrefixPath + "Resources/LevelSO/";

        private const int LOADED_LEVEL_LENGTH = 2;
        private static int _currentLastLevelIndex = 0;
        private static int[] _lastLoadedLevels = new int[LOADED_LEVEL_LENGTH];

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void CheckDirectory()
        {
            if (!Directory.Exists(PrefixPath))
                Directory.CreateDirectory(PrefixPath);
            
            if (!Directory.Exists(ScenePath))
                Directory.CreateDirectory(ScenePath);
            
            if (!Directory.Exists(LevelDataPath))
                Directory.CreateDirectory(LevelDataPath);
        }
        
        public static T CreateLevelManager<T, G>() where T : LevelManagerSo<G> where G : Level
        {
            var levelManager = ScriptableObject.CreateInstance<T>();
            levelManager.name = "Level Manager";
            CheckDirectory();
            AssetDatabase.CreateAsset(levelManager, LevelDataPath + "Level Manager" + ".asset");
            AssetDatabase.SaveAssets();


            return levelManager;
        }

        public static void AddSceneBuildSettings(string scenePath)
        {
            var scenes = EditorBuildSettings.scenes.ToList();
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        public static void RemoveSceneBuildSettings(string scenePath)
        {
            var scenes = EditorBuildSettings.scenes;
            scenes = scenes.Where(I => I.path != scenePath).ToArray();
            EditorBuildSettings.scenes = scenes;
        }
#endif
        
        public static T CreateLevelInstance<T>(int index) where T : Level
        {
            var levelData = ScriptableObject.CreateInstance<T>();
            levelData.name = "Level " + (index + 1);
            levelData.index = index;

            return levelData;
        }

        public static bool HasLevelManagerSo()
        {
            var levelManager = Resources.Load<ScriptableObject>("LevelSO/Level Manager");
            return levelManager != null;
        }

        public static ScriptableObject GetLevelManagerSo()
        {
            var levelManager = Resources.Load<ScriptableObject>("LevelSO/Level Manager");
            return levelManager;
        }

        public static int GetRandomLevelIndex(int min, int max, int levelCount)
        {
            if (levelCount < 5 || max - min < LOADED_LEVEL_LENGTH + 1)
                return Random.Range(min, max);
            
            int level;
            bool flag;
            do
            {
                flag = false;
                level = Random.Range(min, max);
                for (int i = 0; i < _lastLoadedLevels.Length; i++)
                {
                    if (_lastLoadedLevels[i] == level)
                    {
                        flag = true;
                        break;
                    }
                }

            } while (flag);

            _currentLastLevelIndex = (++_currentLastLevelIndex) % LOADED_LEVEL_LENGTH;
            _lastLoadedLevels[_currentLastLevelIndex] = level;
            
            return level;
        }
    }
}
