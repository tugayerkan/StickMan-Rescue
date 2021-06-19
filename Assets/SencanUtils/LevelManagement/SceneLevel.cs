using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SencanUtils.LevelManagement
{
    public class SceneLevel : Level
    {
        private string _sceneName;

        public override void Initialize()
        {
            _sceneName = "Level " + (index + 1);
        }

        public override void LoadLevel()
        {
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive)
                .completed += (operation) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
            };
        }

        public AsyncOperation LoadLevelAsync()
        { 
            return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        }

        public override void UnLoadLevel()
        {
            SceneManager.UnloadSceneAsync(_sceneName);
        }
        
        public AsyncOperation UnloadLevelAsync()
        {
            return SceneManager.UnloadSceneAsync(_sceneName);
        }

#if UNITY_EDITOR
        public override void DeleteLevel()
        {
            var path = Helper.LevelManagementHelper.ScenePath + "Level " + (index + 1) + ".unity";
            Helper.LevelManagementHelper.RemoveSceneBuildSettings(path);
            AssetDatabase.DeleteAsset(path);
        }
        
#endif
    }
}
