using UnityEditor;
using UnityEngine;
using DartCore.Utilities;
using System;

namespace DartCore.Localization
{
    public class LanguageFileManager : EditorWindow
    {
        [MenuItem("DartCore/Localization/Add or Remove Languages")]
        private static void OpenWindow()
        {
            var window = ScriptableObject.CreateInstance<LanguageFileManager>();
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

            EditorScriptingUtils.BeginCenter();
            GUILayout.Label("Current Language(s)", EditorStyles.largeLabel);
            EditorScriptingUtils.EndCenter();

            EditorScriptingUtils.HorizontalLine(3);

            var languageButtonStyle = new GUIStyle(EditorStyles.miniButton);
            languageButtonStyle.fixedHeight = 30f;
            languageButtonStyle.fixedWidth = position.width * .9f;
            languageButtonStyle.fontSize = 12;
            languageButtonStyle.padding = new RectOffset(10, 10, 5, 5);
            languageButtonStyle.alignment = TextAnchor.MiddleCenter;

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
            foreach (var language in languages)
            {
                EditorScriptingUtils.BeginCenter();
                if (GUILayout.Button(language.ToString(), languageButtonStyle))
                {
                    int input = EditorUtility.DisplayDialogComplex(
                        $"What do you want to do with {language} language",
                        $"Do you want to remove or switch to {language} language?",
                        "Switch",
                        "Delete",
                        "Nothing, go back"
                        );
                    if (input == 0)
                    {
                        bool dialogOutput = EditorUtility.DisplayDialog(
                                $"Current language will be set to {language}",
                                $"Are you sure you want to switch to {language}?",
                                "Yes",
                                "No");

                        if (dialogOutput)
                            Localizator.UpdateLanguage(language);
                    }
                    else if (input == 1)
                    {
                        bool dialogOutput = EditorUtility.DisplayDialog(
                            $"{language} language will be removed from the project permanently",
                            $"Are you sure you want to remove {language} language from your project?",
                            "Yes",
                            "No");

                        if (dialogOutput)
                            Localizator.RemoveLanguage(language);
                    }
                    
                }
                EditorScriptingUtils.EndCenter();
            }
            GUILayout.EndScrollView();

            EditorScriptingUtils.HorizontalLine(3);

            // Language
            GUILayout.BeginHorizontal();
            GUILayout.Label("Language to add: ");
            languageToAdd = (SystemLanguage)EditorGUILayout.EnumPopup(languageToAdd, GUILayout.Width(200f));
            GUILayout.EndHorizontal();

            // File Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("File Name: ");
            languageFileName = GUILayout.TextField(languageFileName, GUILayout.Width(200f));
            languageFileName = languageFileName.Replace(' ','_').ToLower();
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
                    bool dialogOutput = EditorUtility.DisplayDialog(
                                $"{languageToAdd} language file will be created",
                                "Are you sure you want to add this language ?",
                                "Add",
                                "Cancel");

                    if (dialogOutput)
                        Localizator.CreateLanguage(languageToAdd, languageFileName.Replace('.','_'), languageName, localizationErrorMessage);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The file name entered or the desired language is invalid", MessageType.Warning);
            }
            
        }
    }
}