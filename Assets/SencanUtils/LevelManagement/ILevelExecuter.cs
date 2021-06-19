namespace SencanUtils.LevelManagement
{
    public interface ILevelExecuter
    {
        int CurrentLevel { get;}
        bool IsAnyLevelLoading { get; }

        void LoadLevel();
        void LoadNextLevel();
        void RestartLevel(bool isLevelFailed);
    }
}
