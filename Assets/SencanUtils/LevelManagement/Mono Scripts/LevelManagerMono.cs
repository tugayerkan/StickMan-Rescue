using UnityEngine;

namespace SencanUtils.LevelManagement.Mono_Scripts
{
    public class LevelManagerMono : MonoBehaviour
    {
        private ILevelExecuter _levelExecuter;

        private void Awake()
        {
            
        }

        private void Start()
        {
            _levelExecuter.LoadLevel();
        }

        public void LoadNextLevel()
        {
            _levelExecuter.LoadNextLevel();
        }

        public void RestartLevel(bool isFailed)
        {
            _levelExecuter.RestartLevel(isFailed);
        }
    }
}
