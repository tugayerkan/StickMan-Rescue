using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MusaUtils
{
    public class Cookies : MonoBehaviour
    {
        public static bool QuickChance(int chance = 10)
        {
            return Random.Range(0, chance) == 0;
        }

        public static GameObject QuickFind(string word = "Player")
        {
            return GameObject.FindGameObjectWithTag(word) == null ? GameObject.Find(word) : GameObject.FindGameObjectWithTag(word);
        }

        public static void LevelUp(string prefName = "Level")
        {
            var i = PlayerPrefs.GetInt(prefName);
            i++;
            PlayerPrefs.SetInt(prefName, i);
        }

        public static void QuickScene(string sceneName = "0")
        {
            PlayerPrefs.Save();
            if (sceneName == "0")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public class QuickRay : MonoBehaviour
    {
        private static RaycastHit _hit;
        private static Ray _ray;
        private static Vector3 _pos;

        public static Vector3 Point(Camera camera, float height = 1f)
        {
            _ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(_ray, out _hit, Mathf.Infinity);
            _pos = _hit.point;
            _pos.y = height;
            return _pos;
        }
        
        private static GameObject _obj;
        
        public static GameObject Object(Transform _spawnPoint)
        {
            _ray = new Ray(_spawnPoint.position, _spawnPoint.forward);
            Physics.Raycast(_ray, out _hit, Mathf.Infinity);
            _obj = _hit.transform.gameObject;
            return _obj;
        }
    }

    public class QuickPatrol : MonoBehaviour
    {
        //************ PATROL METHODS BEGIN ************\\

        private static Vector3 _patrolPoint;
        private static float xPos, zPos;
        private static Rigidbody _rigidbody;
        private static bool isStopped = false;

        public static void StartPatrol(GameObject _gameObject /*Object to Move*/,
            Transform _area /*Movable Area*/,
            float speed = 5f, float waitingDelay = .5f)
        {

            if (!_gameObject.TryGetComponent(out _rigidbody))
            {
                _rigidbody = _gameObject.AddComponent<Rigidbody>();
                QuickBody.GetRigid(_rigidbody).FreezeRotation();
            }
            else
            {
                _rigidbody = _gameObject.GetComponent<Rigidbody>();
            }

            SetPoint(_area);
            Move(_gameObject, _area, speed, waitingDelay);
        }

        public static void StopPatrol()
        {
            isStopped = true;
        }

        private static void SetPoint(Transform _area)
        {
            xPos = _area.localScale.x / 2;
            zPos = _area.localScale.z / 2;
            _patrolPoint.x = Random.Range(-xPos, xPos);
            _patrolPoint.z = Random.Range(-zPos, zPos);
        }

        private static async void Move(GameObject _gameObject, Transform _area, float speed, float delay)
        {
            _gameObject.transform.DOLookAt(_patrolPoint, .25f);
            QuickBody.GetRigid(_rigidbody).GoForward(_gameObject.transform.forward, speed);
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));

            if (Vector3.Distance(_gameObject.transform.position, _patrolPoint) <= 3f)
            {
                _rigidbody.velocity = Vector3.zero;
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
                SetPoint(_area);
            }

            if (!isStopped)
            {
                Move(_gameObject, _area, speed, delay);
            }
        }

        //************ PATROL METHODS END ************\\
    }

    public class QuickInput : MonoBehaviour
    {
        private static Vector2 mousePos;
        private static bool _right;
        
        public static Vector2 GetMouse()
        {
            mousePos = Input.mousePosition;
            return mousePos;
        }
    }
}