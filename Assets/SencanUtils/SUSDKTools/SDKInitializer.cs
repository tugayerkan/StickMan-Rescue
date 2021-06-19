using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUSDKTools
{
    public class SDKInitializer : MonoBehaviour
    {
        private void Start()
        {
            if(!SDKToolHelper.IsAvailablePlatform())
                return;
            
            GameAnalytics.Initialize();
            SDKEvents.Initialize();
            
            FB.Init(() =>
            {
                if(FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("FB is not initialized");
                
                OnInitCompleted();
            });
        }

        private void OnInitCompleted()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
