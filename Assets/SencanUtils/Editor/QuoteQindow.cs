#if UNITY_EDITOR

using System;
using System.Text;
using SencanUtils.Rest;
using SencanUtils.SaveUtils;
using UnityEditor;
using UnityEngine;

namespace SencanUtils.Editor
{
    public class QuoteWindow : EditorWindow
    {
        private const string SAVE_NAME = "QuoteWindowOpened";
        private const int OPEN_FREQUENCY_HOURS = 4;
        private static TimeSpan lastOpenedTime;

        private static QuoteWindow window;
        private static float min, max;

        private static GUIStyle labelStyle, authorStyle;
        
        private static Quote quote;

        private static void OpenWindow()
        {
            quote = APIHelper.GetData<Quote>("https://api.quotable.io/random");
            
            if (quote == null)
            {
                EditorApplication.update -= OpenWindow;

                lastOpenedTime = DateTime.Now.TimeOfDay;
                SaveSystem.Save(SAVE_NAME, lastOpenedTime, SaveSystem.SaveType.BINARY,
                    SaveSystem.SavePathType.Game_Nonerasable);

                return;
            }

            if (quote.length >= 150)
            {
                var split = quote.content.Split(' ');
                var builder = new StringBuilder();
                for (int i = 0; i < split.Length; i++)
                {
                    builder.Append(split[i]);
                    builder.Append(" ");
                    if (i != 0 && i % 20 == 0)
                        builder.Append('\n');
                }

                quote.content = builder.ToString();
            }

            window = (QuoteWindow) GetWindow(typeof(QuoteWindow), true, "Quote");

            labelStyle = new GUIStyle
            {
                fontSize = 15, fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.MiddleCenter,
                stretchWidth = true, richText = true
            };
            authorStyle = new GUIStyle()
            {
                fontSize = 16, fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.LowerCenter, richText = true
            };

            quote.content = "<color=white>" + quote.content + "</color>";
            quote.author = "<color=yellow>" + quote.author + "</color>";
            window.Show();

            EditorApplication.update -= OpenWindow;

            lastOpenedTime = DateTime.Now.TimeOfDay;
            SaveSystem.Save(SAVE_NAME, lastOpenedTime, SaveSystem.SaveType.BINARY,
                SaveSystem.SavePathType.Game_Nonerasable);
        }


        private void OnGUI()
        {
            EditorStyles.label.CalcMinMaxWidth(new GUIContent(quote.content), out min, out max);
            window.minSize = new Vector2(max + 200, 125);
            window.maxSize = new Vector2(max + 300, 125);
            
            EditorGUILayout.Space(25);
            EditorGUILayout.LabelField(quote.content, labelStyle);
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField(quote.author, authorStyle);
        }

        [InitializeOnLoad]
        private static class AppHook
        {
            static AppHook()
            {
                EditorApplication.delayCall += OnInspectorLoaded;
            }

            private static void OnInspectorLoaded()
            {
                lastOpenedTime = SaveSystem.Load<TimeSpan>(SAVE_NAME, SaveSystem.SaveType.BINARY, SaveSystem.SavePathType.Game_Nonerasable);
                TimeSpan diff = DateTime.Now.TimeOfDay - lastOpenedTime;
                
                if (!SessionState.GetBool(SAVE_NAME, false) || (!EditorApplication.isPlayingOrWillChangePlaymode && diff.Duration() > TimeSpan.FromHours(OPEN_FREQUENCY_HOURS)))
                {
                    SessionState.SetBool(SAVE_NAME, true);
                    EditorApplication.update += OpenWindow;
                }
            }
        }
    }
}

#endif
