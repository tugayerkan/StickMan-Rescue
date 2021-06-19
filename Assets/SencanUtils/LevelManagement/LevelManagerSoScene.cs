#if UNITY_EDITOR
using SencanUtils.LevelManagement.Mono_Scripts;
using UnityEditor;  
    using UnityEditor.SceneManagement;
#endif
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

namespace SencanUtils.LevelManagement
{
    public class LevelManagerSoScene : LevelManagerSo<SceneLevel>
    {
        [SerializeField] private bool showLoadingScreen;
        private const string LoadingSceneName = "LoadingScene";

        public override void LoadLevel()
        {
            if (_levels[CurrentLevel].GetLevelName() != SceneManager.GetActiveScene().name)
            {
                base.LoadLevel();
                LoadCurrentLevel();
            }
        }
        
        public override async void LoadNextLevel()
        {
            base.LoadNextLevel(); 
            await _levels[PreviousLevel].UnloadLevelAsync();
            LoadCurrentLevel();
        }

        public override async void RestartLevel(bool isLevelFailed = true)
        {
            base.RestartLevel(isLevelFailed);
            await _levels[CurrentLevel].UnloadLevelAsync();
            LoadCurrentLevel();
        }

        private async void LoadCurrentLevel()
        {
            if (showLoadingScreen)
            {
                await SceneManager.LoadSceneAsync(LoadingSceneName, LoadSceneMode.Additive);
                await UniTask.DelayFrame(1);

                await _levels[CurrentLevel].LoadLevelAsync();
                await UniTask.DelayFrame(1);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_levels[CurrentLevel].GetLevelName()));
                await SceneManager.UnloadSceneAsync(LoadingSceneName);
                _isAnyLevelLoading = false;
            }
            else
            {
                _levels[CurrentLevel].LoadLevel();
            }
        }

#if UNITY_EDITOR
        public override void CreateLevels(int count = 5)
        {
            base.CreateLevels(count);
            
            Scene mainScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(mainScene, Helper.LevelManagementHelper.ScenePath + "MainScene.unity");
            Helper.LevelManagementHelper.AddSceneBuildSettings(mainScene.path);
            
            for (int i = 0; i < count; i++)
            {
                var level = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                EditorSceneManager.SaveScene(level, Helper.LevelManagementHelper.ScenePath + "Level " + (i + 1) + ".unity");
                Helper.LevelManagementHelper.AddSceneBuildSettings(level.path);
                
                var levelData = Helper.LevelManagementHelper.CreateLevelInstance<SceneLevel>(i);
                AssetDatabase.CreateAsset(levelData, Helper.LevelManagementHelper.LevelDataPath + "LevelData " + (i + 1) + ".asset");
                _levels.Add(levelData.index, levelData);
            }
            
            CreateLoadingScene();
        }
        
        public override void RegenerateLevels(int count)
        {           
            base.RegenerateLevels(count);
            
            if (count > LevelCount)
            {
                var diff = count - LevelCount;
                for (int i = 0; i < diff; i++)
                {
                    var level = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                    EditorSceneManager.SaveScene(level, Helper.LevelManagementHelper.ScenePath + "Level " + (LevelCount + 1) + ".unity");
                    Helper.LevelManagementHelper.AddSceneBuildSettings(level.path);
                    
                    var levelData = Helper.LevelManagementHelper.CreateLevelInstance<SceneLevel>(LevelCount);
                    AssetDatabase.CreateAsset(levelData, Helper.LevelManagementHelper.LevelDataPath + "LevelData " + (LevelCount + 1) + ".asset");
                    _levels.Add(levelData.index, levelData);
                }
            }
            else if(count < LevelCount)
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

        private void CreateLoadingScene()
        {
            var loadingScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(loadingScene, Helper.LevelManagementHelper.ScenePath + "LoadingScene.unity");
            Helper.LevelManagementHelper.AddSceneBuildSettings(loadingScene.path);
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
