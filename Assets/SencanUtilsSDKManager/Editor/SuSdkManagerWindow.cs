#if UNITY_EDITOR

using System.Collections;
using System.IO;
using System.Reflection;
using SencanUtilsSDKManager.Core;
using SencanUtilsSDKManager.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SencanUtilsSDKManager.Editor
{
    
    public class SuSdkManagerWindow : EditorWindow
    {
        private enum SdkType
        {
            SU,
            MU,
            SUSdkManager
        }

        private const int SdkCount = 2;
        private bool _sdkManagerHasUpdate;
        
        private EditorCoroutines.EditorCoroutine _downloadCoroutine;
        
        private UnityWebRequest _downloader;
        private SdkPaths _sdkPaths;

        private string _currentSUVersion;
        private Sdk _lastSUVersion;

        private string _currentMUVersion;
        private Sdk _lastMUVersion;

        private string _currentSUSdkManagerVersion;
        private Sdk _lastSUSdkManagerVersion;

        private string _currentActivity;

        private GUIStyle _titleStyle;
        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _buttonStyle;

        [MenuItem("Sencan Utils/Sdk Manager")]
        private static void OpenWindow()
        {
            SuSdkManagerWindow sdkManagerWindow = (SuSdkManagerWindow)GetWindow(typeof(SuSdkManagerWindow), true, "SU SDK Manager");
            sdkManagerWindow.minSize = new Vector2(700, 500);
            sdkManagerWindow.maxSize = new Vector2(700, 500);

            sdkManagerWindow.Focus();
        }

        private void Awake()
        {
            _titleStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter,
                margin = new RectOffset(0, 0, 10, 0),
                fixedHeight = 30,
                stretchHeight = true,
                stretchWidth = true
            };

            _headerStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
            };

            _labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal
            };

            _buttonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fontSize = 12
            };
        }
        
        private void OnEnable()
        {          
            if (File.Exists(SdkManagerHelper.DownloadDirectory + "/Saves/AssetsPath.json"))
            {
                var json = File.ReadAllText(SdkManagerHelper.DownloadDirectory + "/Saves/AssetsPath.json");
                _sdkPaths = JsonUtility.FromJson<SdkPaths>(json);
            }
            _downloadCoroutine = EditorCoroutines.StartCoroutine(DownloadSdkData(SdkType.SUSdkManager), this);
            _downloadCoroutine = EditorCoroutines.StartCoroutine(DownloadSdkData(SdkType.SU), this);
            _downloadCoroutine = EditorCoroutines.StartCoroutine(DownloadSdkData(SdkType.MU), this);
        }

        private void OnDisable()
        {
            CancelOperation();
        }

        private void OnGUI()
        {
            var stillWorking = _downloadCoroutine != null || _downloader != null;

            EditorGUILayout.LabelField("Sencan Utils", _titleStyle, GUILayout.Height(30));
            for (int i = 0; i < SdkCount; i++)
            {
                using (new EditorGUILayout.VerticalScope("Box"))
                {
                    if(_lastSUVersion != null)
                        DrawSDkVersions(i == 0 ? SdkType.SU : SdkType.MU);
                }
            }

            if(_lastSUSdkManagerVersion != null)
                DrawFooter();
        }

        private void DrawSDkVersions(SdkType type)
        {
            using (new EditorGUILayout.VerticalScope("Box"))
            {
                DrawHeaders();
                DrawSdk(type);
            }
        }
        private void DrawHeaders(int width = 200)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Package", _headerStyle, GUILayout.Width(width));
                GUILayout.Label("Current Version", _headerStyle, GUILayout.Width(width));
                GUILayout.Label("Latest Version", _headerStyle, GUILayout.Width(width));
                GUILayout.Label("Action", _headerStyle, GUILayout.Width(width));
            }
        }

        private void DrawSdk(SdkType type, int width = 200)
        {
            Sdk sdk = GetSdk(type);
            var currentVersion = GetCurrentVersion(type);
            
            var sdkName = sdk.name;
            var latestVersion = sdk.version;
            var stillWorking = _downloadCoroutine != null || _downloader != null;
            var isInst = !string.IsNullOrEmpty(currentVersion);
            var canInst = !string.IsNullOrEmpty(latestVersion) &&
                          (!isInst || SdkManagerHelper.CompareVersions(currentVersion, latestVersion) < 0);

            EditorGUILayout.Space(10);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(sdk.name, _labelStyle, GUILayout.Width(width));
                EditorGUILayout.LabelField(currentVersion, _labelStyle, GUILayout.Width(width));
                EditorGUILayout.LabelField(latestVersion, _labelStyle, GUILayout.Width(width));
                
                GUI.enabled = !_sdkManagerHasUpdate && !stillWorking && canInst && !SdkManagerHelper.IsEqualVersions(currentVersion, latestVersion);
                if (GUILayout.Button(new GUIContent {text = isInst ? "Upgrade" : "Install",}, GUILayout.Width(width / 3f)))
                {
                    _downloadCoroutine = EditorCoroutines.StartCoroutine(DownloadSdk(type, sdk), this);
                }
                GUI.enabled = true;
            }
        }
        
        private void DrawFooter(int width = 250)
        {
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope("Box"))
            {
                GUILayout.Space(width / 5f);
                
                EditorGUILayout.LabelField(_currentSUSdkManagerVersion, GUILayout.Width(width));
                
                EditorGUILayout.LabelField(_lastSUSdkManagerVersion.version, GUILayout.Width(width));
                GUI.enabled = _downloader == null && _downloadCoroutine == null && !SdkManagerHelper.IsEqualVersions(_currentSUSdkManagerVersion, _lastSUSdkManagerVersion.version);
                if (GUILayout.Button(new GUIContent
                {
                    text = "Update",
                }, _buttonStyle, GUILayout.Width(width / 3f)))
                    _downloadCoroutine = EditorCoroutines.StartCoroutine(DownloadSdk(SdkType.SUSdkManager, _lastSUSdkManagerVersion), this);
                GUI.enabled = true;
            }
        }

        private void CheckSdkVersion(SdkType type)
        {
            Sdk sdk = GetSdk(type);
            if (Directory.Exists(SdkManagerHelper.AssetPathPrefix + sdk.name))
            {
                var path = sdk.name + "/" + sdk.name + ".dll";
                Assembly assembly = Assembly.LoadFile(SdkManagerHelper.AssetPathPrefix + path);
                foreach (var exportedType in assembly.GetExportedTypes())
                {
                    path = sdk.name + "." + sdk.name;
                    if (exportedType.FullName != null && exportedType.FullName.Equals(path))
                    {
                        var fieldInfo = exportedType.GetField("Version",
                            BindingFlags.Public | BindingFlags.Static);
                        if (type == SdkType.SU)
                            _currentSUVersion = fieldInfo?.GetValue(null).ToString();
                        else if(type == SdkType.MU)
                            _currentMUVersion = fieldInfo?.GetValue(null).ToString();
                        else
                            _currentSUSdkManagerVersion = fieldInfo?.GetValue(null).ToString();
                    }
                }
            }
            else
            {
                if (type == SdkType.SU)
                    _currentSUVersion = string.Empty;
                else if (type == SdkType.MU)
                    _currentMUVersion = string.Empty;
                else
                    _currentSUSdkManagerVersion = string.Empty;
            }

            if (type == SdkType.SUSdkManager)
                _sdkManagerHasUpdate = !SdkManagerHelper.IsEqualVersions(_lastSUSdkManagerVersion.version, _currentSUSdkManagerVersion);
        }

        private IEnumerator DownloadSdkData(SdkType type)
        {
            yield return null;
            using (var request = new UnityWebRequest(GetSdkUrl(type)))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.timeout = 30;
                
                yield return request.SendWebRequest();

                var responseText = request.downloadHandler.text;
                if (!string.IsNullOrEmpty(responseText))
                {
                    if (type == SdkType.SU)
                        _lastSUVersion = JsonUtility.FromJson<Sdk>(request.downloadHandler.text);
                    else if(type == SdkType.MU)
                        _lastMUVersion = JsonUtility.FromJson<Sdk>(request.downloadHandler.text);
                    else
                        _lastSUSdkManagerVersion = JsonUtility.FromJson<Sdk>(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Unable to retrieve SDK version data.");
                    yield break;
                }
                request.Dispose();
            }

            yield return null;
            CheckSdkVersion(type);
            _downloadCoroutine = null;
        }


        private IEnumerator DownloadSdk(SdkType sdkType, Sdk sdk)
        {
            var path = Path.Combine(SdkManagerHelper.AssetPathPrefix, sdk.name + ".unitypackage");
            _currentActivity = $"Downloading {sdk.name}...";
            
            _downloader = new UnityWebRequest(sdk.downloadUrl)
            {
                downloadHandler = new DownloadHandlerFile(path),
                timeout = 180
            };
            _downloader.SendWebRequest();
            
            while (!_downloader.isDone)
            {
                yield return null;
                var progress = Mathf.FloorToInt(_downloader.downloadProgress * 100f);
                if (EditorUtility.DisplayCancelableProgressBar(sdk.name, _currentActivity, progress)) 
                    _downloader.Abort();
            }
            
            EditorUtility.ClearProgressBar();

            if (string.IsNullOrEmpty(_downloader.error))
            {
                if (sdkType == SdkType.SUSdkManager)
                {
                    if (Directory.Exists(SdkManagerHelper.AssetPathPrefix + sdk.name))
                    {
                        FileUtil.DeleteFileOrDirectory(SdkManagerHelper.AssetPathPrefix + sdk.name);
                    }
                }
                else if(sdkType == SdkType.SU)
                {
                    foreach (var sdkPath in _sdkPaths.paths)
                    {
                        if (Directory.Exists(sdkPath))
                        {
                            FileUtil.DeleteFileOrDirectory(sdkPath);
                        }
                    }
                }
                else
                {
                    if (Directory.Exists(SdkManagerHelper.AssetPathPrefix + sdk.name))
                    {
                        FileUtil.DeleteFileOrDirectory(SdkManagerHelper.AssetPathPrefix + sdk.name);
                    }
                }

                AssetDatabase.Refresh();
                AssetDatabase.ImportPackage(path, true);
                FileUtil.DeleteFileOrDirectory(path);
            }
            
            SuPackageManager.DownloadAllPackages();
            
            _downloader.Dispose();
            _downloader = null;
            _downloadCoroutine = null;

            yield return null;
        }

        private void CancelOperation()
        {
            if (_downloader != null)
            {
                _downloader.Abort();
                return;
            }
            
            if (_downloadCoroutine != null)
                this.StopCoroutine(_downloadCoroutine.routine);

            _downloadCoroutine = null;
            _downloader = null;
        }

        private Sdk GetSdk(SdkType type)
        {
            if (type == SdkType.SU)
                return _lastSUVersion;
            if (type == SdkType.MU)
                return _lastMUVersion;
            
            return _lastSUSdkManagerVersion;
        }
        
        private string GetCurrentVersion(SdkType type)
        {
            if (type == SdkType.SU)
                return _currentSUVersion;
            if (type == SdkType.MU)
                return _currentMUVersion;

            return _currentSUSdkManagerVersion;
        }
        
        private string GetSdkUrl(SdkType type)
        {
            if (type == SdkType.SU)
                return SdkUrls.SuSdkDataUrl;
            if (type == SdkType.MU)
                return SdkUrls.MUSdkDataUrl;

            return SdkUrls.SdkManagerDataUrl;
        }
    }
}
#endif