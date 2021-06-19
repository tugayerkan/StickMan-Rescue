#if UNITY_EDITOR
using SencanUtils.LevelManagement.Mono_Scripts;
using UnityEditor;
    using UnityEditor.SceneManagement;
#endif
    using UnityEngine;
using UnityEngine.SceneManagement;

namespace SencanUtils.LevelManagement
{
    public class LevelManagerSoGameObject : LevelManagerSo<GameObjectLevel>
    {
        private string _scenePath;

        protected override void Initialize()
        {
            base.Initialize();
            _scenePath = SceneManager.GetSceneByName("GameScene").path;
        }
        
        public override void LoadLevel()
        {
            base.LoadLevel();
            _levels[CurrentLevel].LoadLevel();
            _isAnyLevelLoading = false;
        }
        
        public override void LoadNextLevel()
        {
            base.LoadNextLevel();
            _levels[CurrentLevel].UnLoadLevel();
            SceneManager.LoadScene(SceneManager.GetSceneByPath(_scenePath).name);
            _levels[CurrentLevel].LoadLevel();
            _isAnyLevelLoading = false;
        }

        public override void RestartLevel(bool isLevelFailed = true)
        {
            base.RestartLevel(isLevelFailed);
            _levels[CurrentLevel].UnLoadLevel();
            SceneManager.LoadScene(SceneManager.GetSceneByPath(_scenePath).name);
            _levels[CurrentLevel].LoadLevel();
            _isAnyLevelLoading = false;
        }

#if UNITY_EDITOR        
        public override void CreateLevels(int count = 5)
        {
            base.CreateLevels(count);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, Helper.LevelManagementHelper.ScenePath + "GameScene" + ".unity");
            _scenePath = scene.path;
            Helper.LevelManagementHelper.AddSceneBuildSettings(_scenePath);

            var parentLevel = new GameObject("Levels")
            {
                tag = "SencanUtils/Level"
            };
            SceneManager.MoveGameObjectToScene(parentLevel, scene);
            for (int i = 0; i < count; i++)
            {
                var levelObject = new GameObject("Level " + (i + 1));
                SceneManager.MoveGameObjectToScene(levelObject, scene);
                levelObject.transform.SetParent(parentLevel.transform);
                levelObject.SetActive(false);
                var levelData = Helper.LevelManagementHelper.CreateLevelInstance<GameObjectLevel>(i);
                AssetDatabase.CreateAsset(levelData, Helper.LevelManagementHelper.LevelDataPath + "LevelData " + (i + 1) + ".asset");
                _levels.Add(levelData.index, levelData);
            }
        }

        public override void RegenerateLevels(int count)
        {
            base.RegenerateLevels(count);

            GameObject parentLevel = GameObject.FindWithTag("SencanUtils/Level");
            if (count > LevelCount)
            {
                var diff = count - LevelCount;
                for (int i = 0; i < diff; i++)
                {
                    var levelData = Helper.LevelManagementHelper.CreateLevelInstance<GameObjectLevel>(LevelCount);
                    AssetDatabase.CreateAsset(levelData, Helper.LevelManagementHelper.LevelDataPath + "LevelData " + (LevelCount + 1) + ".asset");
                    
                    var levelObject = new GameObject("Level " + (LevelCount + 1));
                    SceneManager.MoveGameObjectToScene(levelObject, SceneManager.GetSceneByPath(_scenePath));
                    levelObject.transform.SetParent(parentLevel.transform);
                    levelObject.SetActive(false);
                    _levels.Add(levelData.index, levelData);
                }
            }
            else if (count < LevelCount)
            {
                var diff = LevelCount - count;
                for (int i = 0; i < diff; i++)
                {
                    var level = _levels[LevelCount - 1];
                    level.DeleteLevel();
                    AssetDatabase.DeleteAsset(Helper.LevelManagementHelper.LevelDataPath + "LevelData " + (LevelCount) + ".asset");
                    _levels.RemoveAt(LevelCount - 1);
                }
            }
        }
        
#endif
        
        public override void OnLevelCompleted()
        {
            base.OnLevelCompleted();
        }

        public override void OnLevelFailed()
        {
            base.OnLevelFailed();
        }
    }
}
