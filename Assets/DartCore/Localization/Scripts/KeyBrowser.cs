using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DartCore.Localization
{
    public class KeyBrowser : EditorWindow
    {
        [MenuItem("DartCore/Localization/Key Browser")]
        private static void OpenWindow()
        {
            var window = new KeyBrowser();
            window.titleContent = new GUIContent("Key Browser");
            window.minSize = new Vector2(300, 300);
            window.Show();
        }

        public string[] keys;
        public SystemLanguage[] currentLanguages;
        public const int buttonHeight = 30;
        public const int elementHeight = 16;

        public void OnEnable()
        {
            UpdateArrays();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh", GUILayout.Height(buttonHeight)))
                UpdateArrays();

            GUILayout.BeginVertical();
            for (int i = 0; i < keys.Length; i++)
            {
                GUILayout.BeginArea(new Rect(0, buttonHeight + (i + 1) * elementHeight, position.width, elementHeight));
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(keys[i], GUILayout.Height(elementHeight), GUILayout.Width(90)))
                {
                    KeyEditor.OpenWindow();
                }
                foreach (var language in currentLanguages)
                {
                    string localizedValue = Localizator.GetString(keys[i], language, false);
                    GUILayout.Label($"{language.ToString()} : {(localizedValue == "" ? "-" : "+")}", GUILayout.Height(elementHeight));
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
                GUILayout.Space(20);
            }
            GUILayout.EndVertical();
        }

        private void UpdateArrays()
        {
            keys = Localizator.GetKeys();
            currentLanguages = Localizator.GetAvailableLanguages();
        }

    }
}