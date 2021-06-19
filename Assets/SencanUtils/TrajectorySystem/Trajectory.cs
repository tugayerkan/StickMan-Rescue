using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SencanUtils.TrajectorySystem
{
    [RequireComponent(typeof(LineRenderer))]
    public class Trajectory : MonoBehaviour
    {
        private class TrajectoryMonoHandler : MonoBehaviour
        {
            private static TrajectoryMonoHandler instance;
            public static TrajectoryMonoHandler Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<TrajectoryMonoHandler>();
                        if(instance == null)
                            instance = new GameObject("TrajectoryMonoHandler").AddComponent<TrajectoryMonoHandler>();
                    }
                    return instance;
                }
            }
            
            public bool IsInitialized { get; private set; }

            private PhysicsScene physicsScene;

            public void Initialize()
            {
                IsInitialized = true;
                Physics.autoSimulation = false;
                physicsScene = SceneManager.GetActiveScene().GetPhysicsScene();
            }

            private void FixedUpdate()
            {
                if(!Physics.autoSimulation)
                    physicsScene.Simulate(Time.fixedDeltaTime);
            }

            private void OnDestroy()
            {
                IsInitialized = false;
                Physics.autoSimulation = true;
            }
        }

        public enum Type
        {
            WithCollide = 0,
            WithNonCollide
        }

        [SerializeField] private Type trajectoryType = default;
        [SerializeField, Min(10)] public int positionCount = default;
        [SerializeField] private GameObject fireObjectPrefab = default;

        [SerializeField] private Vector3 velocity = default;
        
        [SerializeField] private float force = default;
        [SerializeField] private string environmentTag = string.Empty;

        public Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
        
        public float Force
        {
            get => force;
            set => force = value;
        }
        
        private Scene scene;
        private PhysicsScene physicsScene;

        private LineRenderer lineRenderer;
        
        private GameObject shootableObject;
        private Rigidbody shootableRb;
        

        private void Awake()
        {
            if(!TrajectoryMonoHandler.Instance.IsInitialized)
                TrajectoryMonoHandler.Instance.Initialize();
            
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
        
        public void DrawTrajectory(Transform shootPoint)
        {
            if(trajectoryType == Type.WithCollide)
                DrawTrajectoryWithCollision(shootPoint);
            else if (trajectoryType == Type.WithNonCollide)
                DrawTrajectoryWithMath(shootPoint);
        }

        private void DrawTrajectoryWithMath(Transform shootPoint)
        {
            float t = (-1f * velocity.y) / Physics.gravity.y;
            t = 2f * t;
            
            lineRenderer.positionCount = positionCount;
            Vector3 trajectoryPoint;
            float time;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                time = t * i / (lineRenderer.positionCount);
                trajectoryPoint = shootPoint.position + velocity * time + 0.5f * Physics.gravity * time * time;
                lineRenderer.SetPosition(i, trajectoryPoint);
            }
        }

        private void DrawTrajectoryWithCollision(Transform shootPoint)
        {
            if (!scene.IsValid())
            {
                scene = SceneManager.CreateScene("PhysicsScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
                physicsScene = scene.GetPhysicsScene();
            }
            if (shootableObject == null)
            {
                shootableObject = Instantiate(fireObjectPrefab);
                shootableRb = GetComponent<Rigidbody>();
                var renderers = shootableObject.GetComponentsInChildren<Renderer>();
                foreach (var rend in renderers)
                {
                    rend.enabled = false;
                }
                SceneManager.MoveGameObjectToScene(shootableObject, scene);
            }
            
            shootableObject.SetActive(true);
            shootableObject.transform.position = shootPoint.position;
            shootableObject.transform.rotation = shootPoint.rotation;
            lineRenderer.positionCount = positionCount;
            shootableRb.AddForce(shootPoint.forward * force);
            
            for (int i = 0; i < positionCount; i++)
            {
                lineRenderer.SetPosition(i, shootableObject.transform.position);
                physicsScene.Simulate(Time.fixedDeltaTime);
            }
            
            shootableRb.velocity = Vector3.zero;
            shootableRb.angularVelocity = Vector3.zero;
            shootableObject.SetActive(false);
        }
    }
}
