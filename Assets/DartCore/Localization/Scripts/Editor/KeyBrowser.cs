using DartCore.Utilities;
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
            var window = ScriptableObject.CreateInstance<KeyBrowser>();
            window.titleContent = new GUIContent("Key Browser");
            window.Show();
        }

        public string[] keys;
        private bool keysInit = false;
        public SystemLanguage[] currentLanguages;
        private const int BUTTON_HEIGHT = 30;
        private const int ELEMENT_HEIGHT = 23;
        private const int ELEMENT_WIDTH = 150;

        private Vector2 scrollPos;
        private string search = "";
        private List<string> searchedKeys;
        private LocalizationStatus statusToDisplay = LocalizationStatus.All;

        public void OnEnable()
        {
            minSize = new Vector2(200, 100);
        }

        private void OnGUI()
        {
            // Styles
            var languageDisplayerStyle = new GUIStyle
            {
                richText = true,
                fixedHeight = ELEMENT_HEIGHT,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5)
            };
            var keyButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = ELEMENT_HEIGHT,
                fixedWidth = ELEMENT_WIDTH,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5)
            };
            var keyButtonStyleBold = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = ELEMENT_HEIGHT,
                fixedWidth = ELEMENT_WIDTH,
                fontSize = 12,
                fontStyle = FontStyle.BoldAndItalic,
                padding = new RectOffset(10, 10, 5, 5)
            };

            if (GUILayout.Button("Refresh", GUILayout.Height(BUTTON_HEIGHT)) || !keysInit)
            {
                keysInit = true;
                UpdateArrays();
                Repaint();
            }

            GUILayout.Space(10);

            var searchStyle = new GUIStyle(EditorStyles.textField)
            {
                fixedWidth = position.width * .99f,
                fixedHeight = 23f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };

            search = GUILayout.TextField(search, searchStyle);
            search = search.Replace(' ', '_').ToLower();
            Search(search);

            // Search Options
            statusToDisplay = (LocalizationStatus) EditorGUILayout.EnumPopup(statusToDisplay);

            GUILayout.Space(10);
            EditorScriptingUtils.HorizontalLine(3);

            var rectPos = EditorGUILayout.GetControlRect();
            var rectBox = new Rect(rectPos.x, rectPos.y, rectPos.width, position.height - 110);
            var viewRect = new Rect(rectPos.x, rectPos.y, (1 + currentLanguages.Length) * ELEMENT_WIDTH, searchedKeys.Count * ELEMENT_HEIGHT);

            scrollPos = GUI.BeginScrollView(rectBox, scrollPos, viewRect, false, true);

            var viewCount = Mathf.FloorToInt((position.height - 110) / ELEMENT_HEIGHT);
            var firstIndex = Mathf.FloorToInt(scrollPos.y / ELEMENT_HEIGHT);

            var contentPos = new Rect(rectBox.x, firstIndex * ELEMENT_HEIGHT + 80f, rectBox.width, ELEMENT_HEIGHT);

            Localizator.UpdateKeyFile();
            for (var i = firstIndex; i < Mathf.Min(firstIndex + viewCount, searchedKeys.Count); i++)
            {
                contentPos.y += ELEMENT_HEIGHT;

                var languageLocalizationDict = new Dictionary<SystemLanguage, bool>();
                foreach (var language in currentLanguages)
                    languageLocalizationDict.Add(language,
                        !string.IsNullOrWhiteSpace(Localizator.GetString(searchedKeys[i], language, false)));

                // Filtering
                if (statusToDisplay == LocalizationStatus.Localized &&
                    languageLocalizationDict.Values.Contains(false)) continue;

                if (statusToDisplay == LocalizationStatus.NotLocalized &&
                    !languageLocalizationDict.Values.Contains(false)) continue;

                // Displaying
                EditorGUILayout.BeginHorizontal();

                var currentKey = searchedKeys[i];
                if (GUI.Button(contentPos, currentKey,
                    currentKey == "lng_name" || currentKey == "lng_error" ? keyButtonStyleBold : keyButtonStyle))
                {
                    KeyEditor.key = currentKey;
                    FocusWindowIfItsOpen(typeof(KeyEditor));
                }

                GUILayout.FlexibleSpace();
                for (var j = 0; j < currentLanguages.Length; j++)
                {
                    var language = currentLanguages[j];
                    var offsetedContentPos = new Rect(contentPos.x + (j + 1) * ELEMENT_WIDTH, contentPos.y, contentPos.width, ELEMENT_HEIGHT);
                    GUI.Label(offsetedContentPos,
                        languageLocalizationDict[language]
                            ? $"<color=green>{language.ToString()}</color>"
                            : $"<color=red>{language.ToString()}</color>",
                        languageDisplayerStyle);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUI.EndScrollView();
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
                {
                    if (key.Trim() == "lng_name" || key.Trim() == "lng_error")
                        searchedKeys.Insert(0, key.Trim());
                    else 
                        searchedKeys.Add(key.Trim());
                }
            }
            
        }
    }

    /// <summary>
    /// Used to filter the KeyBrowser's search results.
    /// </summary>
    public enum LocalizationStatus
    {
        All = 0,
        Localized = 1,
        NotLocalized = 2,
    }
}