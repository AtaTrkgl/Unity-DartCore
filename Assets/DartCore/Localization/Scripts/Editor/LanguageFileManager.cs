using UnityEditor;
using UnityEngine;
using DartCore.Utilities;
using System;

namespace DartCore.Localization.Backend
{
    public class LanguageFileManager : EditorWindow
    {
        [MenuItem("DartCore/Localization/Language File Manager")]
        private static void OpenWindow()
        {
            var window = CreateInstance<LanguageFileManager>();
            window.titleContent = new GUIContent("Language File Manager");
            window.Show();
        }

        private Vector2 scrollPos;

        private SystemLanguage languageToAdd;
        private string languageFileName = "";
        private string languageName = "";
        private string localizationErrorMessage = "";

        private void OnEnable()
        {
            minSize = new Vector2(376, 250);
        }

        private void OnGUI()
        {
            var languages = Localizator.GetAvailableLanguages();
            var localizationPercentages = Localizator.GetLocalizationPercentages();

            EditorScriptingUtils.BeginCenter();
            GUILayout.Label("Current Language(s)", EditorStyles.largeLabel);
            EditorScriptingUtils.EndCenter();

            EditorScriptingUtils.HorizontalLine(3);

            var languageButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 30f,
                fixedWidth = position.width * .3f,
                fontSize = 12,
                padding = new RectOffset(10, 10, 5, 5),
                alignment = TextAnchor.MiddleCenter,
                richText = true
            };

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
            foreach (var language in languages)
            {
                EditorScriptingUtils.BeginCenter();
                GUILayout.BeginHorizontal();
                var localizationPercentage = localizationPercentages[language];
                
                // Label
                GUILayout.Label($"<color={(localizationPercentage > 90f ? "green" : "red")}><b>{language.ToString()}</b>, {localizationPercentage}% Localized</color>",
                    languageButtonStyle);
                
                // Switch to the language
                if (GUILayout.Button($"Switch", languageButtonStyle))
                {
                    Localizator.UpdateLanguage(language);
                    Debug.Log($"Switched to the {language} language.");
                }
                
                // Remove the language
                if (GUILayout.Button($"Remove", languageButtonStyle))
                {
                    var dialogOutput = EditorUtility.DisplayDialog(
                        $"{language} language will be removed from the project permanently",
                        $"Are you sure you want to remove {language} language from your project?",
                        "Yes",
                        "No");

                    if (dialogOutput)
                    {
                        Localizator.RemoveLanguage(language);
                        ((KeyBrowser) GetWindow(typeof(KeyBrowser))).UpdateArrays();
                    }

                }

                GUILayout.EndHorizontal();
                EditorScriptingUtils.EndCenter();
            }

            GUILayout.EndScrollView();

            EditorScriptingUtils.HorizontalLine(3);

            // Language
            GUILayout.BeginHorizontal();
            GUILayout.Label("Language to add: ");
            languageToAdd = (SystemLanguage) EditorGUILayout.EnumPopup(languageToAdd, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            // File Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("File Name: ");
            languageFileName = GUILayout.TextField(languageFileName, GUILayout.Width(200f));
            languageFileName = languageFileName.Replace(' ', '_').ToLower();
            GUILayout.EndHorizontal();

            // Language Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("Language Name: ");
            languageName = GUILayout.TextField(languageName, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            // Localization Error Message
            GUILayout.BeginHorizontal();
            GUILayout.Label("Localization Error Message: ");
            localizationErrorMessage = GUILayout.TextField(localizationErrorMessage, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            bool canCreateLanguage = Array.IndexOf(Localizator.GetCurrentLanguageFiles(), languageFileName.Trim()) == -1
                                     && !string.IsNullOrWhiteSpace(languageFileName)
                                     && Array.IndexOf(Localizator.GetAvailableLanguages(), languageToAdd) == -1;

            if (canCreateLanguage)
            {
                if (GUILayout.Button($"Add {languageToAdd} language"))
                {
                    var dialogOutput = EditorUtility.DisplayDialog(
                        $"{languageToAdd} language file will be created",
                        "Are you sure you want to add this language ?",
                        "Add",
                        "Cancel");

                    if (dialogOutput)
                    {
                        Localizator.CreateLanguage(languageToAdd, languageFileName.Replace('.', '_'), languageName,
                            localizationErrorMessage);
                        ((KeyBrowser) GetWindow(typeof(KeyBrowser))).UpdateArrays();
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The file name entered or the desired language is invalid",
                    MessageType.Warning);
            }
        }
    }
}