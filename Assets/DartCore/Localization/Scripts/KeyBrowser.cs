using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DartCore.Localization
{
    public class KeyBrowser : EditorWindow
    {
        [MenuItem("DartCore/Localization/Key Browser")]
        private static void OpenWindow()
        {
            var window = new KeyBrowser();
            window.titleContent = new GUIContent("Key Browser");
            window.minSize = new Vector2(100, 100);
            window.Show();
        }

        public string[] keys;
        public SystemLanguage[] currentLanguages;
        public const int buttonHeight = 30;
        public const int elementHeight = 23;

        private Vector2 scrollPos;
        private GUIStyle languageDisplayerStyle;
        private GUIStyle keyButtonStyle;
        private string search = "";
        private List<string> searchedKeys;

        public void OnEnable()
        {
            UpdateArrays();

            languageDisplayerStyle = new GUIStyle();
            languageDisplayerStyle.richText = true;
            languageDisplayerStyle.fixedHeight = elementHeight;
            languageDisplayerStyle.fontStyle = FontStyle.Bold;
            languageDisplayerStyle.fontSize = 12;
            languageDisplayerStyle.padding = new RectOffset(10, 10, 5, 5);

            keyButtonStyle = new GUIStyle(/*EditorStyles.miniButton*/);
            keyButtonStyle.fixedHeight = elementHeight;
            keyButtonStyle.fontSize = 12;
            keyButtonStyle.padding = new RectOffset(10,10,5,5);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh", GUILayout.Height(buttonHeight)))
            { 
                UpdateArrays();
                GUI.FocusControl(null);
                Repaint();
            }
            GUILayout.Space(10);

            var searchStyle = new GUIStyle(EditorStyles.textField);
            searchStyle.fixedWidth = position.width * .99f;
            searchStyle.fixedHeight = 23f;
            searchStyle.alignment = TextAnchor.MiddleCenter;
            searchStyle.fontSize = 12;

            search = GUILayout.TextField(search, searchStyle);
            search = search.Replace(' ', '_');
            search = search.ToLower();
            Search(search);

            GUILayout.Space(10);
            HorizontalLine(3);

            scrollPos = GUILayout.BeginScrollView(scrollPos,
                false, true);

            for (int i = 0; i < searchedKeys.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(searchedKeys[i], keyButtonStyle))
                {
                    KeyEditor.key = searchedKeys[i];
                    FocusWindowIfItsOpen(typeof(KeyEditor));
                }
                GUILayout.FlexibleSpace();
                foreach (var language in currentLanguages)
                {
                    string localizedValue = Localizator.GetString(searchedKeys[i], language, false);
                    GUILayout.Label((localizedValue == "" ?
                            $"<color=red>{language.ToString()}</color>" :
                            $"<color=green>{language.ToString()}</color>"),
                        languageDisplayerStyle);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void HorizontalLine(int height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;

            EditorGUI.DrawRect(rect, new Color(0.7f, 0.7f, 0.7f, 1));
        }

        private void UpdateArrays()
        {
            keys = Localizator.GetKeys();
            Array.Sort(keys);
            currentLanguages = Localizator.GetAvailableLanguages();
            Repaint();
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