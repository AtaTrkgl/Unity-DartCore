using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

namespace DartCore.Localization
{
    public class Localizator
    {
        private static bool keysArrayInitilized = false;
        private static string[] keysArray;
        private static string[] currentLanguageArray;

        private static Dictionary<SystemLanguage, string> languageNames;
        private static bool lngDictIsInitilized = false;

        private static SystemLanguage currentLanguage = SystemLanguage.English;

        private const string LNG_FILES_PATH = "Assets/DartCore/Localization/Resources/";
        private const string KEYS_FILE_NAME = "_keys";
        private const string LNG_NAMES_FILE = "_lng_names";

        public static string GetString(string key, bool returnErrorString = true)
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            if (!keysArrayInitilized)
            {
                UpdateKeyFile();
                LoadLanguageFile();
                keysArrayInitilized = true;
            }

            int index = GetIndexOfKey(key);
            bool doesLngFileContainsKey = currentLanguageArray.Length > index && index >= 0;

            if (doesLngFileContainsKey && !string.IsNullOrWhiteSpace(currentLanguageArray[index].Trim()))
                return currentLanguageArray[index].Trim();

            return returnErrorString ? currentLanguageArray[1].Trim() : "";
        }

        public static string GetString(string key, SystemLanguage language, bool returnErrorString = true)
        {
            if (!keysArrayInitilized)
            {
                UpdateKeyFile();
                keysArrayInitilized = true;
            }

            if (!lngDictIsInitilized)
            {
                LoadLanguageFile();
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            if (!languageNames.ContainsKey(language))
                return "";

            var languageArray = Resources.Load<TextAsset>(languageNames[language]).text.Split('\n');

            var index = GetIndexOfKey(key);
            bool doesLngFileContainsKey = languageArray.Length > index;

            if (doesLngFileContainsKey)
            {
                if (index == -1 && returnErrorString)
                    return languageArray[1];
                else if (index == -1)
                    return "";
                else
                    return languageArray[index];
            }
            else if (returnErrorString)
                return languageArray[1];
            else
                return "";
        }

        public static void UpdateKeyFile()
        {
            keysArray = ReadAllLines(KEYS_FILE_NAME);
        }

        public static void AddKey(string key)
        {
            key = key.Replace('\n', new char());
            key = key.Replace(' ', '_');
            if (!DoesContainKey(key))
            {
                File.AppendAllText(LNG_FILES_PATH + KEYS_FILE_NAME + ".txt", "\n" + key);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
                UpdateKeyFile();
            }
        }

        public static void RemoveKey(string key)
        {
            if (key == "lng_name" || key == "lng_error")
            {
                Debug.LogError($"You can not remove the \"{key}\" key");
                return;
            }

            key = key.Replace('\n', new char()).Replace(' ', '_');
            if (DoesContainKey(key))
            {
                if (!lngDictIsInitilized)
                {
                    UpdateLanguageDictionary();
                    lngDictIsInitilized = true;
                }

                int index = GetIndexOfKey(key);

                // KEY REMOVAL
                string newText = "";
                for (int i = 0; i < keysArray.Length; i++)
                {
                    if (i != index)
                        newText += keysArray[i] + "\n";
                }

                newText = newText.Remove(newText.Length - 1);
                File.WriteAllText(LNG_FILES_PATH + KEYS_FILE_NAME + ".txt", newText);

                // VALUE REMOVAL
                foreach (var language in languageNames.Keys)
                {
                    newText = "";
                    for (int i = 0; i < keysArray.Length; i++)
                    {
                        if (i != index)
                            newText += GetString(keysArray[i], language) + "\n";
                    }

                    newText = newText.Remove(newText.Length - 1);
                    File.WriteAllText(LNG_FILES_PATH + languageNames[language] + ".txt", newText);
                }
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            UpdateKeyFile();
            LoadLanguageFile();
        }

        public static void AddLocalizedValue(string key, string localizedValue, SystemLanguage language)
        {
            if (GetString(key, language, true) != localizedValue)
            {
                if (!lngDictIsInitilized)
                {
                    UpdateLanguageDictionary();
                    lngDictIsInitilized = true;
                }

                localizedValue = localizedValue.Replace('\n', new char()).Trim();
                string value = File.ReadAllText(LNG_FILES_PATH + languageNames[language] + ".txt");

                var index = GetIndexOfKey(key);
                string endString = "";

                var splittedValue = value.Split('\n');
                int iterCount = index > splittedValue.Length - 1 ? index + 1 : splittedValue.Length;
                for (int i = 0; i < iterCount; i++)
                {
                    if (i != 0)
                        endString += "\n";

                    if (index == i)
                        endString += localizedValue;
                    else if (splittedValue.Length > i)
                        endString += splittedValue[i];
                }

                File.WriteAllText(LNG_FILES_PATH + languageNames[language] + ".txt", endString);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }

        private static void LoadLanguageFile()
        {
            UpdateLanguageDictionary();
            lngDictIsInitilized = true;

            currentLanguageArray = ReadAllLines(languageNames[currentLanguage]);
        }

        public static bool UpdateLanguage(SystemLanguage language)
        {
            if (!languageNames.ContainsKey(language))
                return false;

            currentLanguage = language;
            LoadLanguageFile();

            OnLanguageChange?.Invoke();

            return true;
        }

        public static void SetLanguageAccordingToSystem()
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            var language = Application.systemLanguage;
            if (languageNames.ContainsKey(language))
                UpdateLanguage(language);
        }

        public static int GetLanguageCount()
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            return languageNames.Count;
        }

        public static SystemLanguage[] GetAvailableLanguages()
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            SystemLanguage[] languages = new SystemLanguage[languageNames.Keys.Count];
            for (int i = 0; i < languageNames.Count; i++)
                languages[i] = languageNames.Keys.ElementAt(i);

            return languages;
        }

        public static string[] GetCurrentLanguageFiles()
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            string[] languageFiles = new string[languageNames.Values.Count];
            for (int i = 0; i < languageNames.Count; i++)
                languageFiles[i] = languageNames.Values.ElementAt(i);

            return languageFiles;
        }

        public static bool DoesContainKey(string key)
        {
            if (!keysArrayInitilized)
            {
                UpdateKeyFile();
                keysArrayInitilized = true;
            }

            return GetIndexOfKey(key) != -1;
        }

        private static int GetIndexOfKey(string key)
        {
            for (int i = 0; i < keysArray.Length; i++)
            {
                if (keysArray[i].Trim() == key.Trim())
                    return i;
            }

            return -1;
        }

        public static string[] GetKeys()
        {
            UpdateKeyFile();
            return keysArray;
        }

        public static void CreateLanguage(SystemLanguage language, string fileName, string lngName,
            string lngErrorMessage)
        {
            fileName = fileName.Trim().Replace(' ', '_');
            if (lngName == "")
                lngName = language.ToString();
            if (lngErrorMessage == "")
                lngErrorMessage = $"Localization Error ({lngName})";

            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            if (!languageNames.ContainsKey(language) && !languageNames.ContainsValue(fileName))
            {
                File.WriteAllText(LNG_FILES_PATH + fileName + ".txt", lngName.Trim() + "\n" + lngErrorMessage.Trim());
                var lines = File.ReadAllText(LNG_FILES_PATH + LNG_NAMES_FILE + ".txt").Split('\n');
                string text = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == (int) language)
                        text += fileName + "\n";
                    else
                        text += lines[i] + "\n";
                }

                text = text.Remove(text.Length - 1);
                File.WriteAllText(LNG_FILES_PATH + LNG_NAMES_FILE + ".txt", text);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
                UpdateLanguageDictionary();
            }
        }

        public static void RemoveLanguage(SystemLanguage language)
        {
            if (languageNames.Values.Count == 1)
            {
                Debug.LogError("You can not remove the only language available");
                return;
            }

            var lines = File.ReadAllText(LNG_FILES_PATH + LNG_NAMES_FILE + ".txt").Split('\n');
            lines[(int) language] = "";
            string text = "";
            foreach (var line in lines)
                text += line + "\n";
            text = text.Remove(text.Length - 1);
            File.WriteAllText(LNG_FILES_PATH + LNG_NAMES_FILE + ".txt", text);

            File.Delete(LNG_FILES_PATH + languageNames[language] + ".txt");

            languageNames.Remove(language);
        }

        private static void UpdateLanguageDictionary()
        {
            languageNames = new Dictionary<SystemLanguage, string>();
            var lines = ReadAllLines(LNG_NAMES_FILE);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    languageNames.Add((SystemLanguage) i, lines[i].Trim());
                }
            }

            if (!languageNames.ContainsKey(currentLanguage))
            {
                currentLanguage = languageNames.Keys.ElementAt(0);
                Debug.Log($"Changed the current language to {currentLanguage}");
            }
        }

        private static string[] ReadAllLines(string fileName)
        {
            var file = Resources.Load<TextAsset>(fileName);
            return file.text.Split('\n');
        }

        public static SystemLanguage GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public static void RefreshAll()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            UpdateKeyFile();
            LoadLanguageFile();
        }

        public delegate void LanguageChange();

        public static event LanguageChange OnLanguageChange;

        public static Dictionary<SystemLanguage, float> GetLocalizationPercentages()
        {
            var dict = new Dictionary<SystemLanguage, float>();
            foreach (var language in GetAvailableLanguages())
            {
                var lines = File.ReadAllText(LNG_FILES_PATH + languageNames[language] + ".txt").Split('\n');
                var filledRowCount = 0;

                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line))
                        filledRowCount++;

                dict.Add(language, Mathf.Round(100f * filledRowCount / lines.Length));
            }

            return dict;
        }
    }
}