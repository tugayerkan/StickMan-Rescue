using UnityEngine;

namespace SencanUtils.LevelManagement
{
    public class GameObjectLevel : Level
    {
        private GameObject _parentLevel;
        
        public override void Initialize()
        {
            _parentLevel = GetParentLevel();
        }

        public override void LoadLevel()
        {
            if (_parentLevel == null)
                _parentLevel = GetParentLevel();
            
            _parentLevel.transform.GetChild(index).gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        
        public override void DeleteLevel()
        {
            if (_parentLevel == null)
                _parentLevel = GetParentLevel();
            
            DestroyImmediate(_parentLevel.transform.GetChild(index).gameObject);
        }
        
#endif

        private GameObject GetParentLevel()
        {
            return GameObject.FindWithTag("SencanUtils/Level");
        }
    }
}
