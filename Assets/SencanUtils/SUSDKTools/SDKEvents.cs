using Facebook.Unity;
using GameAnalyticsSDK;
using SencanUtils.LevelManagement;
using SencanUtils.SUSDKTools;

namespace SUSDKTools
{
    public static class SDKEvents
    {
        private static SdkParam _sdkParams;
        
        public static void Initialize()
        {
            LevelManagerSo<Level>.onLevelStarted += LevelStarted;
            LevelManagerSo<Level>.onLevelCompleted += LevelCompleted;
            LevelManagerSo<Level>.onLevelFailed += LevelCompleted;

            _sdkParams = new SdkParam();
        }
        
        private static void LevelStarted(Level level)
        {
            if(!SDKToolHelper.IsAvailablePlatform())
                return;

            _sdkParams.Clear();
            _sdkParams.AddParameter(level.GetLevelName(), level.sectionDescription);
            
            FB.LogAppEvent("LevelStarted", null, _sdkParams.GetParameters());
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + level.UIIndex, level.sectionDescription);
        }

        private static void LevelCompleted(Level level)
        {
            if(!SDKToolHelper.IsAvailablePlatform())
                return;
            
            _sdkParams.Clear();
            _sdkParams.AddParameter(level.GetLevelName(), level.sectionDescription);
            
            FB.LogAppEvent("LevelCompleted", null, _sdkParams.GetParameters());
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,"Level " + level.UIIndex, level.sectionDescription);
        }

        private static void LevelFailed(Level level)
        {
            if(!SDKToolHelper.IsAvailablePlatform())
                return;
            
            _sdkParams.Clear();
            _sdkParams.AddParameter(level.GetLevelName(), level.sectionDescription);
            
            FB.LogAppEvent("LevelFailed", null, _sdkParams.GetParameters());
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + level.UIIndex, level.sectionDescription);
        }
    }
}

