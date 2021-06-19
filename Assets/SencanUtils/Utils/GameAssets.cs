using UnityEngine;

namespace SencanUtils
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _instance;
        public static GameAssets Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (Resources.Load(nameof(GameAssets)) as GameObject)?.GetComponent<GameAssets>();
            
                return _instance;
            }
        }

        //Assets Here
    }
}
