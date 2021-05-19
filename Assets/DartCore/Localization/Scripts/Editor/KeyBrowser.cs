using DartCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DartCore.Localization.Backend
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
        private const int LANGUAGE_NAME_DISPLAYER_WIDTH = 100;

        private Vector2 scrollPos;
        private string search = "";
        private string lastSearch = ".";
        private List<string> searchedKeys;
        private List<string> keysWithCorrectLocalizationStatus;
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
                Refresh();
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
            SetLocalizationStatus((LocalizationStatus) EditorGUILayout.EnumPopup(statusToDisplay));

            GUILayout.Space(10);
            EditorScriptingUtils.HorizontalLine(3);

            var rectPos = EditorGUILayout.GetControlRect();
            var rectBox = new Rect(rectPos.x, rectPos.y, rectPos.width, position.height - 110);
            var viewRect = new Rect(rectPos.x, rectPos.y, 
                currentLanguages.Length * LANGUAGE_NAME_DISPLAYER_WIDTH + ELEMENT_WIDTH, searchedKeys.Count * ELEMENT_HEIGHT);

            scrollPos = GUI.BeginScrollView(rectBox, scrollPos, viewRect, false, true);

            var viewCount = Mathf.FloorToInt((position.height - 110) / ELEMENT_HEIGHT);
            var firstIndex = Mathf.FloorToInt(scrollPos.y / ELEMENT_HEIGHT);

            var contentPos = new Rect(rectBox.x, firstIndex * ELEMENT_HEIGHT + 80f, rectBox.width, ELEMENT_HEIGHT);

            Localizator.UpdateKeyFile();
            
            for (var i = firstIndex; i < Mathf.Min(firstIndex + viewCount, searchedKeys.Count); i++)
            {
                contentPos.y += ELEMENT_HEIGHT;

                var localizationStatus = Localizator.GetLocalizationStatusOfKey(searchedKeys[i]);
                
                // Displaying
                EditorGUILayout.BeginHorizontal();

                var currentKey = searchedKeys[i];
                if (GUI.Button(contentPos, currentKey,
                    currentKey == "lng_name" || currentKey == "lng_error" ? keyButtonStyleBold : keyButtonStyle))
                {
                    KeyEditor.key = currentKey;
                    
                    var window = (KeyEditor) GetWindow( typeof(KeyEditor), false,"Key Editor",true);
                    window.Show();
                    FocusWindowIfItsOpen(typeof(KeyEditor));
                }

                GUILayout.FlexibleSpace();
                for (var j = 0; j < currentLanguages.Length; j++)
                {
                    var language = currentLanguages[j];
                    var xOffset = ELEMENT_WIDTH + j * LANGUAGE_NAME_DISPLAYER_WIDTH;
                    var offsetedContentPos = new Rect(contentPos.x + xOffset, contentPos.y, contentPos.width, ELEMENT_HEIGHT);
                    GUI.Label(offsetedContentPos,
                        localizationStatus[j]
                            ? $"<color=green>{language.ToString()}</color>"
                            : $"<color=red>{language.ToString()}</color>",
                        languageDisplayerStyle);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUI.EndScrollView();
        }

        public void Refresh()
        {
            keysInit = true;
            UpdateArrays();
            Repaint();
            Localizator.RefreshAll();
        }

        private void SetLocalizationStatus(LocalizationStatus status)
        {
            if (status == statusToDisplay) return;
            
            statusToDisplay = status;
            FilterKeys();
        }

        private void FilterKeys()
        {
            if (statusToDisplay == LocalizationStatus.All)
            {
                keysWithCorrectLocalizationStatus = keys.ToList();
            }
            else
            {
                keysWithCorrectLocalizationStatus = new List<string>();
            
                foreach (var key in keys)
                {
                    var isLocalized = !Localizator.GetLocalizationStatusOfKey(key).Contains(false);
                    
                    if (statusToDisplay == LocalizationStatus.Localized && isLocalized ||
                        statusToDisplay == LocalizationStatus.NotLocalized && !isLocalized)
                        keysWithCorrectLocalizationStatus.Add(key);
                }
            }
            
            Search("", ignoreRepetition: true);
        }
        
        public void UpdateArrays()
        {
            Localizator.RefreshAll();
            keys = Localizator.GetKeysArray();
            Array.Sort(keys);
            currentLanguages = Localizator.GetAvailableLanguages();
            
            Search("", ignoreRepetition: true);
            FilterKeys();
        }

        private void Search(string search, bool ignoreRepetition = false)
        {
            if (search == lastSearch && !ignoreRepetition) return;
            
            searchedKeys = new List<string>();

            if (keysWithCorrectLocalizationStatus == null) FilterKeys();
            foreach (var key in keysWithCorrectLocalizationStatus)
            {
                if (key.Trim().Contains(search) || search == "")
                {
                    if (key.Trim() == "lng_name" || key.Trim() == "lng_error")
                        searchedKeys.Insert(0, key.Trim());
                    else 
                        searchedKeys.Add(key.Trim());
                }
            }

            lastSearch = search;
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