using DartCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

        private Vector2 scrollPos;
        private GUIStyle languageDisplayerStyle;
        private string search = "";
        private List<string> searchedKeys;
        private LocalizationStatus statusToDisplay = LocalizationStatus.All;

        public void OnEnable()
        {
            minSize = new Vector2(200, 100);
        }

        private void OnGUI()
        {
            languageDisplayerStyle = new GUIStyle
            {
                richText = true,
                fixedHeight = ELEMENT_HEIGHT,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
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

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

            var keyButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = ELEMENT_HEIGHT,
                fixedWidth = 150f,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5)
            };
            var keyButtonStyleBold = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = ELEMENT_HEIGHT,
                fixedWidth = 150f,
                fontSize = 12,
                fontStyle = FontStyle.BoldAndItalic,
                padding = new RectOffset(10, 10, 5, 5)
            };

            Localizator.UpdateKeyFile();
            foreach (var searchedKey in searchedKeys)
            {
                var languageLocalizationDict = new Dictionary<SystemLanguage, bool>();
                foreach (var language in currentLanguages)
                    languageLocalizationDict.Add(language,
                        !string.IsNullOrWhiteSpace(Localizator.GetString(searchedKey, language, false)));

                // Filtering
                if (statusToDisplay == LocalizationStatus.Localized &&
                    languageLocalizationDict.Values.Contains(false)) continue;

                if (statusToDisplay == LocalizationStatus.NotLocalized &&
                    !languageLocalizationDict.Values.Contains(false)) continue;

                // Displaying
                GUILayout.BeginHorizontal();

                var currentKey = searchedKey;
                if (GUILayout.Button(currentKey,
                    currentKey == "lng_name" || currentKey == "lng_error" ? keyButtonStyleBold : keyButtonStyle))
                {
                    KeyEditor.key = currentKey;
                    FocusWindowIfItsOpen(typeof(KeyEditor));
                }

                GUILayout.FlexibleSpace();
                foreach (var language in currentLanguages)
                {
                    GUILayout.Label(
                        languageLocalizationDict[language]
                            ? $"<color=green>{language.ToString()}</color>"
                            : $"<color=red>{language.ToString()}</color>",
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