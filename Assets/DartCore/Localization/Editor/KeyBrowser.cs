using DartCore.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DartCore.Localization
{
    public class KeyBrowser : EditorWindow
    {
        [MenuItem("DartCore/Localization/Key Browser")]
        private static void OpenWindow()
        {
            var window = ScriptableObject.CreateInstance<KeyBrowser>();
            window.titleContent = new GUIContent("Key Browser");
            window.Show();
        }

        public string[] keys;
        private bool keysInit = false;
        public SystemLanguage[] currentLanguages;
        public const int buttonHeight = 30;
        public const int elementHeight = 23;

        private Vector2 scrollPos;
        private GUIStyle languageDisplayerStyle;
        private string search = "";
        private List<string> searchedKeys;

        public void OnEnable()
        {
            minSize = new Vector2(200, 100);
        }

        private void OnGUI()
        {
            languageDisplayerStyle = new GUIStyle
            {
                richText = true,
                fixedHeight = elementHeight,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5)
            };

            if (GUILayout.Button("Refresh", GUILayout.Height(buttonHeight)) || !keysInit)
            {
                keysInit = true;
                UpdateArrays();
                Repaint();
            }

            GUILayout.Space(10);

            var searchStyle = new GUIStyle(EditorStyles.textField);
            searchStyle.fixedWidth = position.width * .99f;
            searchStyle.fixedHeight = 23f;
            searchStyle.alignment = TextAnchor.MiddleCenter;
            searchStyle.fontSize = 12;

            search = GUILayout.TextField(search, searchStyle);
            search = search.Replace(' ', '_').ToLower();
            Search(search);

            GUILayout.Space(10);
            EditorScriptingUtils.HorizontalLine(3);

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

            var keyButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = elementHeight,
                fixedWidth = 100f,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5)
            };
            var keyButtonStyleBold = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = elementHeight,
                fixedWidth = 100f,
                fontSize = 12,
                fontStyle = FontStyle.BoldAndItalic,
                padding = new RectOffset(10, 10, 5, 5)
            };

            Localizator.UpdateKeyFile();
            for (int i = 0; i < searchedKeys.Count; i++)
            {
                GUILayout.BeginHorizontal();

                var currentKey = searchedKeys[i];
                if (GUILayout.Button(currentKey, (currentKey == "lng_name" || currentKey == "lng_error") ? keyButtonStyleBold : keyButtonStyle))
                {
                    KeyEditor.key = currentKey;
                    FocusWindowIfItsOpen(typeof(KeyEditor));
                }
                GUILayout.FlexibleSpace();
                foreach (var language in currentLanguages)
                {
                    string localizedValue = Localizator.GetString(currentKey, language, false);
                    GUILayout.Label(localizedValue == "" ?
                            $"<color=red>{language.ToString()}</color>" :
                            $"<color=green>{language.ToString()}</color>",
                        languageDisplayerStyle);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void UpdateArrays()
        {
            Localizator.RefreshAll();
            keys = Localizator.GetKeys();
            Array.Sort(keys);
            currentLanguages = Localizator.GetAvailableLanguages();
        }

        private void Search(string search)
        {
            searchedKeys = new List<string>();

            foreach (var key in keys)
            {
                if (key.Trim().Contains(search) || search == "")
                    searchedKeys.Add(key.Trim());
            }
        }

    }
}