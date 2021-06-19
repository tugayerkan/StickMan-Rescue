
namespace SencanUtils.LevelManagement
{
    public interface ILevelEventHandler
    {
        void OnLevelCompleted();
        void OnLevelFailed();
    }
}
