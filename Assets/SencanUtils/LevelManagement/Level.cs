using UnityEngine;

namespace SencanUtils.LevelManagement
{
    public abstract class Level : ScriptableObject
    {
        public int index;
        public int UIIndex => index + 1;
        
        public string sectionDescription;

        public abstract void Initialize();

        public abstract void LoadLevel();

        public virtual void UnLoadLevel() { }

#if UNITY_EDITOR
        public abstract void DeleteLevel();
#endif
        public virtual string GetLevelName() => "Level " + (index + 1);
    }
}
