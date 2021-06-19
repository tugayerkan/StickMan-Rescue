
namespace SencanUtils.LevelManagement
{
    public interface ILevelCreator
    {
#if UNITY_EDITOR
        int LevelCount { get; }
        void CreateLevels(int levelCount);
        void RegenerateLevels(int levelCount);
#endif
    }
}
