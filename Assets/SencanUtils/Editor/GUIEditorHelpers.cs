#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class GUIEditorHelpers
{
    //Gets the target Gameobject in the scene
    public static GameObject GetTargetSceneObject(GameObject currentObj)
    {

        currentObj = (GameObject)EditorGUILayout.ObjectField(currentObj, typeof(GameObject), allowSceneObjects: true);

        return currentObj;
    }
    //selects a Monobehavior on the targeted GameObject
    public static MonoBehaviour GetTargetMono(GameObject TargetObj, MonoBehaviour currentMono)
    {
      
        MonoBehaviour[] ms = TargetObj.GetComponents<MonoBehaviour>();
 
        if (ms.Length == 0)
        {
            return null;
        }

        //set to 0
        int currentIndex = 0;
        if (currentMono != null)
        {
        
            Type currentT = currentMono.GetType();
            for (int i = 0; i < ms.Length; i++)
            {
                if (currentT == ms[i].GetType())
                {
                    currentIndex = i;
                    break;
                }
            }
        }
 
        string[] names = GetObjectNames(ms);

      
        currentIndex = EditorGUILayout.Popup(currentIndex, names);
 
        return ms[currentIndex];
    }

    //selects a method from the selected mono
    public static MethodInfo GUIMethodeSelect(MonoBehaviour targetMono, string currentFunction)
    {
        MethodInfo[] mis = targetMono.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

        string[] s = new string[mis.Length];
        int cIndex = 0;

        //get the Method infos as a string array
        for (int i = 0; i < s.Length; i++) 
            s[i] = mis[i].Name;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == currentFunction)
            {
                cIndex = i;
                break;
            }
        }

        return mis[EditorGUILayout.Popup(cIndex, s)];
    }
    public static string[] GetObjectNames(object[] obs)
    {
        string[] names = new string[obs.Length];
        for (int i = 0; i < obs.Length; i++)
        {
            names[i] = obs[i].GetType().ToString();
        }
        return names;
    }
}


#endif