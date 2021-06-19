#if UNITY_EDITOR

using System.Text;
using UnityEditor;
using UnityEngine;

namespace SencanUtils.Editor
{
    public class MeshSimplifierWindow : EditorWindow
    {
        private Object source;
        private float quality;
        private string meshName = "Mesh Name";

        public static void OpenWindow()
        {
            var window = GetWindow(typeof(MeshSimplifierWindow), true, "Mesh Simplifier");
            window.minSize = new Vector2(350, 150);
            window.maxSize = new Vector2(500, 200);
            window.Show();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Simplified Mesh", GUILayout.MaxWidth(100));
            source = EditorGUILayout.ObjectField(source, typeof(Mesh), false);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Quality", GUILayout.MaxWidth(100));
            quality = EditorGUILayout.Slider(quality, 0f, 1f);
            EditorGUILayout.EndHorizontal();
            
            meshName = EditorGUILayout.TextField("New Saving Mesh Name", meshName);

            if (GUILayout.Button("Simplify And Save!"))
            {
                if (source == null)
                    ShowNotification(new GUIContent("No object selected for searching"));
                else
                {
                    var mesh = (Mesh) source;
                    var simplifier = new UnityMeshSimplifier.MeshSimplifier();
                    simplifier.Initialize(mesh);
                    simplifier.SimplifyMesh(quality);
                    mesh = simplifier.ToMesh();

                    var splitPath = AssetDatabase.GetAssetPath(source).Split('/');
                    var path = new StringBuilder();
                    for (var i = 0; i < splitPath.Length - 1; i++)
                    {
                        path.Append(splitPath[i]);
                        path.Append("/");
                    }
                    
                    path.Append("/Simplified-");
                    path.Append(quality);
                    path.Append("-");
                    path.Append(meshName);
                    path.Append(".mesh");

                    AssetDatabase.CreateAsset(mesh, path.ToString());
                    Debug.Log("Simplifying Successful in Path " + path);
                }
            }
        }
    }
}

#endif
