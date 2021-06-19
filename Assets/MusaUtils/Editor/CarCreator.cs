using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MusaUtils.Editor
{
    public class CarCreator : EditorWindow
    {
        private static SerializedObject tagManager = 
            new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        
        private static SerializedProperty tagsProp = tagManager.FindProperty("tags");

        private static GameObject _car;
        private static GameObject _wheel;
        
        
        
        
        [MenuItem("MU/Create Car Controller")]
        private static void AddComponentEditor()
        {
            AddTag("WheelFront");
            AddTag("WheelRear");
                
            //CAR BASE OBJECT
            _car = new GameObject("NewCar");
            
            ObjectFactory.AddComponent<CarController.CarController>(_car);
            
            BoxCollider collider = ObjectFactory.AddComponent<BoxCollider>(_car);
            collider.center = new Vector3(0, .5f, 0);
            collider.size = new Vector3(2.5f, 1f, 4f);
            
            ObjectFactory.AddComponent<Rigidbody>(_car).mass = 200f;

            //WHEELS
            AddWheel("WheelFR", "WheelFront", new Vector3(1.75f, 0, 1.5f));
            
            AddWheel("WheelFL", "WheelFront", new Vector3(-1.75f, 0, 1.5f));
            
            AddWheel("WheelRR", "WheelRear", new Vector3(1.75f, 0, -1.5f));
            
            AddWheel("WheelRL", "WheelRear", new Vector3(-1.75f, 0, -1.5f));
            
            _car.transform.position = Vector3.up;

            if (FindObjectOfType<Canvas>() == null)
            { CreateCanvas(); }
            
            if (FindObjectOfType<EventSystem>() == null)
            { CreateEventSystem(); }

            if (FindObjectOfType<FloatingJoystick>() == null)
            {
                CreateJoystick();
            }
        }

        private static void CreateJoystick()
        {
            Object _joystick = AssetDatabase.LoadAssetAtPath("Assets/MusaUtils/ExtentionAssets/Joystick/Prefabs/Floating.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(
                _joystick, SceneManager.GetActiveScene()
            );
            FindObjectOfType<FloatingJoystick>().transform.parent = FindObjectOfType<Canvas>().transform;
        }

        private static void CreateCanvas()
        {
            GameObject _canvas = new GameObject("Canvas");
            ObjectFactory.AddComponent<Canvas>(_canvas).renderMode = RenderMode.ScreenSpaceOverlay;
            ObjectFactory.AddComponent<GraphicRaycaster>(_canvas);
            var _canvasScaler = ObjectFactory.AddComponent<CanvasScaler>(_canvas);
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 1920);
        }

        private static void CreateEventSystem()
        {
            GameObject _eventSystem = new GameObject("EventSystem");
            ObjectFactory.AddComponent<EventSystem>(_eventSystem);
            ObjectFactory.AddComponent<StandaloneInputModule>(_eventSystem);
        }

        private static void AddTag(string newT)
        {
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(newT))
                {
                    found = true;
                }
            }

            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(0);
                newTag.stringValue = newT;   
                tagManager.ApplyModifiedProperties();
            }
        }

        private static void AddWheel(string wheelName, string wheelTag, Vector3 wheelPos)
        {
            _wheel = new GameObject(wheelName);
            WheelCollider _wSet = ObjectFactory.AddComponent<WheelCollider>(_wheel);
            JointSpring _spring = new JointSpring();
            _spring.spring = 3500f;
            _spring.damper = 450f;
            _wSet.suspensionSpring = _spring;

            _wheel.transform.SetParent(_car.transform);
            _wheel.tag = wheelTag;
            _wheel.transform.position = wheelPos;
        }
    }
}
