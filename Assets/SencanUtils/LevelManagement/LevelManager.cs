using System;
using System.Collections.Generic;
using SencanUtils.SaveUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SencanUtils.LevelManagement
{
    public abstract class LevelManagerSo<T> : ScriptableObject, ILevelCreator, ILevelExecuter, ILevelEventHandler where T : Level
    {
        public enum RandomType
        {
            RandomLevel,
            RandomLevelBetweenTwoValue
        }

        public static event Action<T> onLevelStarted;
        public static event Action<T> onLevelCompleted;
        public static event Action<T> onLevelFailed;

        [SerializeField] protected RandomType randomTypeAfterLevelsFinished;
        [SerializeField] protected int minRandomValue;
        [SerializeField] protected int maxRandomValue;

        public int LevelCount => _levels.Count;
        public bool IsAnyLevelLoading => _isAnyLevelLoading;

        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                _currentLevel = value;
                SaveSystem.Save("Level", _currentLevel, SaveSystem.SaveType.PlayerPrefs);
            }
        }

        public int PreviousLevel => _previousLevel;

        protected bool HasAllLevelCompleted => SaveSystem.Load<bool>("HasAllLevelCompleted", SaveSystem.SaveType.PlayerPrefs);

        private bool _isInitialized;
        private int _currentLevel;
        private int _previousLevel;
        protected bool _isAnyLevelLoading;
        
        protected SortedList<int, T> _levels;
        
        protected virtual void OnEnable()
        {
            Initialize();
            SceneManager.sceneLoaded += OnLevelLoaded;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }

        protected virtual void Initialize()
        {
            _isInitialized = true;
            _currentLevel = SaveSystem.Load<int>("Level", SaveSystem.SaveType.PlayerPrefs);
            _levels = new SortedList<int, T>();
            var levels = Resources.LoadAll<T>("LevelSO");
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].Initialize();
                _levels.Add(levels[i].index, levels[i]);
            }

            if (minRandomValue == 0 && maxRandomValue == 0)
            {
                minRandomValue = 0;
                maxRandomValue = LevelCount;
            }
        }
        
        protected virtual void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) { }

        public virtual void RestartLevel(bool isLevelFailed = true)
        {
            _isAnyLevelLoading = true;
            if(isLevelFailed)
                onLevelFailed?.Invoke(_levels[_currentLevel]);
            
            onLevelStarted?.Invoke(_levels[CurrentLevel]);
        }

        public virtual void LoadLevel()
        {
            _isAnyLevelLoading = true;
            onLevelStarted?.Invoke(_levels[CurrentLevel]);
        }
        
        public virtual void LoadNextLevel()
        {
            _isAnyLevelLoading = true;
            _previousLevel = _currentLevel;
            if (HasAllLevelCompleted || ++CurrentLevel == LevelCount)
            {
                SaveSystem.Save("HasAllLevelCompleted", true, SaveSystem.SaveType.PlayerPrefs);
                CurrentLevel = Helper.LevelManagementHelper.GetRandomLevelIndex(minRandomValue, maxRandomValue, LevelCount);
            }
        }

#if UNITY_EDITOR
        
        public virtual void CreateLevels(int count = 5)
        {
            _currentLevel = 0;
            _levels = new SortedList<int, T>();
            Helper.LevelManagementHelper.AddSceneBuildSettings("Assets/SencanUtils/SU_Initializer.unity");
        }

        public virtual void RegenerateLevels(int count)
        {
            if(!_isInitialized)
                Initialize();
        }
        
#endif
        
        public virtual void OnLevelCompleted()
        {
            onLevelCompleted?.Invoke(_levels[_currentLevel]);
        }

        public virtual void OnLevelFailed()
        {
            onLevelFailed?.Invoke(_levels[_currentLevel]);
        }
    }
}
