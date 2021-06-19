#if UNITY_EDITOR

using System;
using System.Reflection;
using System.Text;
using SencanUtils.Rest;
using SencanUtils.Runtime;
using UnityEditor;
using UnityEngine;
using SencanUtils.SaveUtils;

namespace SencanUtils.Editor
{
    public class TimeTrack : EditorWindow
    {
        private const int SAVE_FREQUENCY_MINUTE = 1;
        private const int SEND_INFO_FREQUENCY_HOURS = 1;
        
        private static TimeTrackData timeTrackData;
        private static SendInformation sendInformation;

        private static StringBuilder _stringBuilder = new StringBuilder();

        [MenuItem("Utils/SencanUtils/Time Track")]
        private static void ShowWindow()
        {
            var window = GetWindowWithRect(typeof(TimeTrack), new Rect(Vector2.zero, new Vector2(350, 100)), true, "Time Track");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Time Track Infos", EditorStyles.boldLabel);
            TimeSpan displayTime = timeTrackData.totalHoursWorked;
            
            ReadOnlyText("Project Created:", timeTrackData.createdTime.ToString("MM/dd/yyyy - HH:mm"));
            ReadOnlyText("Time worked: ", displayTime.Days + " Days, " + displayTime.Hours + " Hours, " + displayTime.Minutes + " Minutes");
        }

        private void ReadOnlyText(string label, string text)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
            EditorGUILayout.SelectableLabel(text, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            
            EditorGUILayout.EndHorizontal();
        }
        
        private static void SaveData()
        {
            if ((DateTime.Now - timeTrackData.lastSavedTime).Minutes >= SAVE_FREQUENCY_MINUTE)
            {
                timeTrackData.lastSavedTime = DateTime.Now;
                timeTrackData.totalHoursWorked += TimeSpan.FromMinutes(SAVE_FREQUENCY_MINUTE);
                SaveSystem.Save("TimeTrackDataInfo" + Application.productName, timeTrackData, SaveSystem.SaveType.BINARY,
                    SaveSystem.SavePathType.Game_Nonerasable);
                
                if (TimeSpan.FromSeconds(EditorApplication.timeSinceStartup).Hours >= SEND_INFO_FREQUENCY_HOURS && (DateTime.Now - sendInformation.lastSendTime).Hours >= SEND_INFO_FREQUENCY_HOURS)
                {
                    sendInformation.helpTexts = GetHelpText();
                    var bytes = CaptureSceneScreenshotAsPNG(1920, 1080);
                    if(bytes != null)
                        TelegramAPI.SendPhoto(bytes, Application.productName, $"Person: {SystemInfo.deviceName} Project Name: {Application.productName} Session: {TimeSpan.FromSeconds(EditorApplication.timeSinceStartup):hh\\:mm} Enter PlayTime Count(Session): {sendInformation.enterPlayModeCount} Total Work: {timeTrackData.totalHoursWorked:dd\\.hh\\:mm} \n\nProblems\n\n {sendInformation.helpTexts}");
                    else
                        TelegramAPI.SendMessage($"Person: {SystemInfo.deviceName} Project Name: {Application.productName} Session: {TimeSpan.FromSeconds(EditorApplication.timeSinceStartup):hh\\:mm} Enter PlayTime Count(Session): {sendInformation.enterPlayModeCount} Total Work: {timeTrackData.totalHoursWorked:dd\\.hh\\:mm} \n\nProblems\n\n {sendInformation.helpTexts}");
                    
                    sendInformation.lastSendTime = DateTime.Now;
                    sendInformation.enterPlayModeCount = 0;
                    SaveSystem.Save("SendInformationData" + Application.productName, sendInformation, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
                }
            }
        }
        
        private static void PlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                sendInformation.enterPlayModeCount++;
                SaveSystem.Save("SendInformationData" + Application.productName, sendInformation, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
            }
        }

        private static void LoadData()
        {
            if (SaveSystem.HasSaveFile("TimeTrackDataInfo" + Application.productName, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable))
            {
                try
                {
                    var save = SaveSystem.Load<TimeTrackData>("TimeTrackDataInfo" + Application.productName, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
                    timeTrackData = save;
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                    SaveSystem.DeleteAllData(true);
                }
            }
            else
            {
                timeTrackData = new TimeTrackData
                {
                    totalHoursWorked = TimeSpan.FromSeconds(EditorApplication.timeSinceStartup)
                };
                timeTrackData.createdTime = DateTime.Now - timeTrackData.totalHoursWorked;
                SaveSystem.Save("TimeTrackDataInfo" + Application.productName, timeTrackData, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
            }
        }
        
        private static void LoadInformationData()
        {
            if (SaveSystem.HasSaveFile("SendInformationData" + Application.productName, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable))
            {
                try
                {
                    var save = SaveSystem.Load<SendInformation>("SendInformationData" + Application.productName, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
                    sendInformation = save;
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                    SaveSystem.DeleteAllData(true);
                }
            }
            else
            {
                sendInformation = new SendInformation {lastSendTime = DateTime.Now};
                SaveSystem.Save("SendInformationData" + Application.productName, sendInformation, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
            }
        }
        
        private static byte[] CaptureSceneScreenshotAsPNG(int width, int height)
        {
            Camera camera = SceneView.lastActiveSceneView.camera;
            
            RenderTexture renderTex = RenderTexture.GetTemporary(width, height, 24);
            Texture2D screenshot = null;
            try
            {
                RenderTexture.active = renderTex;

                camera.targetTexture = renderTex;
                camera.Render();

                screenshot = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBA32, false);
                screenshot.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0, false);
                screenshot.Apply(false, false);

                return screenshot.EncodeToPNG();
            }
            finally
            {
                if (screenshot != null)
                    DestroyImmediate(screenshot);
            }
        }

        private static string GetHelpText()
        {
            _stringBuilder.Clear();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    NeedHelpAttribute atr = types[j].GetCustomAttribute<NeedHelpAttribute>();
                    if (atr != null)
                    {
                        _stringBuilder.Append("Class Name: ");
                        _stringBuilder.AppendLine(types[j].FullName);
                        _stringBuilder.Append("Problem: ");
                        _stringBuilder.Append(atr.Problem);
                        _stringBuilder.AppendLine("\n");
                    }
                }
            }

            return _stringBuilder.ToString();
        }

        [InitializeOnLoad]
        private class Startup
        {
            static Startup()
            {
                LoadData();
                LoadInformationData();
                EditorApplication.update += SaveData;
                EditorApplication.playModeStateChanged += PlayModeStateChanged;
            }
        }
        
        [Serializable]
        private struct TimeTrackData 
        {
            public DateTime createdTime;
            public TimeSpan totalHoursWorked;
            public DateTime lastSavedTime;
        }
        
        [Serializable]
        private struct SendInformation
        {
            public DateTime lastSendTime;
            public int enterPlayModeCount;

            public string helpTexts;
        }
    }
}
#endif