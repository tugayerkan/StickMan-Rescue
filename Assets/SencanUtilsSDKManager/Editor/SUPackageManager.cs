#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SencanUtilsSDKManager.Core;
using SencanUtilsSDKManager.Util;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace SencanUtilsSDKManager.Editor
{
    public static class SuPackageManager
    {
        private const string PackagesPath = SdkManagerHelper.DownloadDirectory + "Saves/Dependencies.json";
        
        private static Dictionary<string, bool> _dependencyDictionary;
        private static Dictionary<string, Request> _requestDictionary;
        
        private static ListRequest _listRequest;

        private static bool _isCompleted;
        public static bool IsCompleted => _isCompleted;

        public static void DownloadAllPackages()
        {
            _isCompleted = false;
            _requestDictionary = new Dictionary<string, Request>();
            _dependencyDictionary = GetSUDependencies();
            if (_dependencyDictionary == null)
            {
                _isCompleted = true;
                return;
            }

            RequestList();
        }
        
        private static Dictionary<string, bool> GetSUDependencies()
        {
            var dependenciesTup = new Dictionary<string, bool>();
            if (File.Exists(PackagesPath))
            {
                var json = File.ReadAllText(PackagesPath);
                SdkPaths dependencies = JsonUtility.FromJson<SdkPaths>(json);
                foreach (var dependency in dependencies.paths)
                {
                    dependenciesTup.Add(dependency, false);
                }

                return dependenciesTup;
            }

            return null;
        }

        private static void RequestList()
        {
            _listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        private static void RequestProgress()
        {
            if (_requestDictionary.Count == 0)
            {
                _isCompleted = true;
                EditorApplication.update -= RequestProgress;
            }

            for(int i = 0; i < _requestDictionary.Count; i++)
            {
                var pair = _requestDictionary.ElementAt(i);
                if (pair.Value == null)
                {
                    _requestDictionary.Remove(pair.Key);
                    i--;
                }
                else if (pair.Value.IsCompleted)
                {
                    if (pair.Value.Status == StatusCode.Success)
                        Debug.Log("Installed: " + pair.Key);
                    else if (pair.Value.Status >= StatusCode.Failure)
                        Debug.Log(pair.Value.Error.message);

                    _requestDictionary.Remove(pair.Key);
                    i--;
                }
            }
        }
        
        private static void ListProgress()
        {
            if (_listRequest.IsCompleted)
            {
                if (_listRequest.Status == StatusCode.Success)
                {
                    foreach (var package in _listRequest.Result)
                    {
                        if (_dependencyDictionary.ContainsKey(package.name))
                            _dependencyDictionary[package.name] = true;
                        else
                            _requestDictionary.Add(package.name, null);
                    }

                    if (_dependencyDictionary.Count == 0)
                    {
                        _isCompleted = true;
                        return;
                    }

                    LoadPackages();
                }
                else if (_listRequest.Status >= StatusCode.Failure)
                {
                    _isCompleted = true;
                    Debug.Log(_listRequest.Error.message);
                }

                EditorApplication.update -= ListProgress;
            }
        }

        private static void LoadPackages()
        {
            foreach (var dependency in _dependencyDictionary)
            {
                if (!dependency.Value)
                {
                    _requestDictionary[dependency.Key] = Client.Add(dependency.Key);
                }
            }
            EditorApplication.update += RequestProgress;
        }
    }
}

#endif