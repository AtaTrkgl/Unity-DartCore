using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

namespace DartCore.Localization
{
    public class Localizator
    {
        private static string[] keysArray;
        private static Dictionary<SystemLanguage, string[]> languageArrays;
        private static Dictionary<SystemLanguage, string> languageNames;

        private static SystemLanguage currentLanguage = SystemLanguage.English;

        private const string LNG_FILES_PATH = "Assets/DartCore/Localization/Resources/";
        private const string KEYS_FILE_NAME = "_keys";
        private const string LNG_NAMES_FILE = "_lng_names";
        private const string LINE_BREAK_TEXT = "<line_break>";

        /// <summary>
        /// returns an array which consists of all the keys.
        /// </summary>
        public static string[] GetKeysArray() => keysArray ??= ReadAllLines(KEYS_FILE_NAME);

        private static Dictionary<SystemLanguage, string> GetLanguageNames()
        {
            if (languageNames == null)LoadLanguageFile();
            return languageNames;
        }

        private static Dictionary<SystemLanguage, string[]> GetLanguageArrays()
        {
            if (languageArrays == null) LoadLanguageFile();
            return languageArrays;
        }

        /// <summary>
        /// Returns the localized value of the given key in the current language.
        /// </summary>
        public static string GetString(string key, bool returnErrorString = true)
        {
            return GetString(key, currentLanguage, returnErrorString: returnErrorString);
        }
        
        /// <summary>
        /// Returns the localized value of the given key in the specified language.
        /// </summary>
        public static string GetString(string key, SystemLanguage language, bool returnErrorString = true)
        {
            if (!GetLanguageNames().ContainsKey(language))
                return "";

            var languageArray = GetLanguageArrays()[language];
            var index = GetIndexOfKey(key);

            var doesLngFileContainsKey = languageArray.Length > index && index >= 0;

            if (!doesLngFileContainsKey || index == -1)
                return returnErrorString ? ConvertSavedStringToUsableString(languageArray[1]) : "";
            
            return ConvertSavedStringToUsableString(languageArray[index]);
        }

        /// <summary>
        /// Returns a list of booleans that corresponds to the localization status of the key in the available languages.
        /// </summary>
        public static bool[] GetLocalizationStatusOfKey(string key)
        {
            var availableLanguages = GetAvailableLanguages();
            var localizationStatuses = new bool[availableLanguages.Length];

            var keyIndex = GetIndexOfKey(key);
            for (var i = 0; i < availableLanguages.Length; i++)
                localizationStatuses[i] = !string.IsNullOrWhiteSpace(GetLanguageArrays()[availableLanguages[i]][keyIndex]);

            return localizationStatuses;
        }

        public static string GetStringRaw(string key, SystemLanguage language)
        {
            return ConvertUsableStringToSavedString(GetString(key, language, returnErrorString: false));
        }

        private static string ConvertSavedStringToUsableString(string savedString) => savedString.Trim().Replace(LINE_BREAK_TEXT, "\n");
        private static string ConvertUsableStringToSavedString(string usableString) => usableString
            .Replace(Environment.NewLine, LINE_BREAK_TEXT)
            .Replace("\n", LINE_BREAK_TEXT)
            .Trim();

        /// <summary>
        /// Works like DartCore.Localization.GetString(), if there is no localized value for the given key in the current language
        /// returns the localized value in the given fallBackLanguage, if the key is not present in the fallback language returns an
        /// error string with the current language if returnErrorString is set to True else it will just return an empty string.
        /// </summary>
        public static string GetStringWithFallBackLanguage(string key, SystemLanguage fallBackLanguage, bool returnErrorString = true)
        {
            var result = GetString(key, returnErrorString: false);
            if (!string.IsNullOrWhiteSpace(result)) return result.Trim();

            result = GetString(key, fallBackLanguage, returnErrorString: false);
            if (!string.IsNullOrWhiteSpace(result)) return result.Trim();

            return returnErrorString ? GetString("lng_error") : "";
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

                int index = GetIndexOfKey(key);

                // KEY REMOVAL
                string newText = "";
                for (int i = 0; i < GetKeysArray().Length; i++)
                {
                    if (i != index)
                        newText += GetKeysArray()[i] + (i != GetKeysArray().Length - 1 ? "\n" : "");
                }

                File.WriteAllText(LNG_FILES_PATH + KEYS_FILE_NAME + ".txt", newText);

                // VALUE REMOVAL
                foreach (var language in GetLanguageNames().Keys)
                {
                    newText = "";
                    for (int i = 0; i < GetKeysArray().Length; i++)
                    {
                        if (i != index)
                            newText += GetStringRaw(GetKeysArray()[i], language) + "\n";
                    }

                    newText = newText.Remove(newText.Length - 1);
                    File.WriteAllText(LNG_FILES_PATH + GetLanguageNames()[language] + ".txt", newText);
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
                localizedValue = ConvertUsableStringToSavedString(localizedValue);
                string value = File.ReadAllText(LNG_FILES_PATH + GetLanguageNames()[language] + ".txt");

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

                File.WriteAllText(LNG_FILES_PATH + GetLanguageNames()[language] + ".txt", endString);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }

        private static void LoadLanguageFile()
        {
            UpdateLanguageDictionary();

            var languages = GetAvailableLanguages();

            languageArrays = new Dictionary<SystemLanguage, string[]>();
            for (var i = 0; i < languages.Length; i++)
                languageArrays[languages[i]] = ReadAllLines(GetLanguageNames()[languages[i]]);
        }

        public static bool UpdateLanguage(SystemLanguage language)
        {
            if (!GetLanguageNames().ContainsKey(language))
                return false;

            currentLanguage = language;
            LoadLanguageFile();

            OnLanguageChange?.Invoke();

            return true;
        }

        public static void SetLanguageAccordingToSystem()
        {
            var language = Application.systemLanguage;
            if (GetLanguageNames().ContainsKey(language))
                UpdateLanguage(language);
        }

        public static int GetLanguageCount() => GetLanguageNames().Count;

        public static SystemLanguage[] GetAvailableLanguages()
        {
            var languages = new SystemLanguage[GetLanguageNames().Keys.Count];
            for (var i = 0; i < GetLanguageNames().Count; i++)
                languages[i] = GetLanguageNames().Keys.ElementAt(i);

            return languages;
        }

        public static string[] GetCurrentLanguageFiles()
        {
            var languageFiles = new string[GetLanguageNames().Values.Count];
            for (var i = 0; i < GetLanguageNames().Count; i++)
                languageFiles[i] = GetLanguageNames().Values.ElementAt(i);

            return languageFiles;
        }

        public static bool DoesContainKey(string key)
        {
            return GetIndexOfKey(key) != -1;
        }

        private static int GetIndexOfKey(string key)
        {
            for (int i = 0; i < GetKeysArray().Length; i++)
            {
                if (GetKeysArray()[i].Trim() == key.Trim())
                    return i;
            }

            return -1;
        }

        public static void CreateLanguage(SystemLanguage language, string fileName, string lngName,
            string lngErrorMessage)
        {
            fileName = fileName.Trim().Replace(' ', '_');
            if (lngName == "")
                lngName = language.ToString();
            if (lngErrorMessage == "")
                lngErrorMessage = $"Localization Error ({lngName})";

            if (!GetLanguageNames().ContainsKey(language) && !GetLanguageNames().ContainsValue(fileName))
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
            if (GetLanguageNames().Values.Count == 1)
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

            File.Delete(LNG_FILES_PATH + GetLanguageNames()[language] + ".txt");

            GetLanguageNames().Remove(language);
        }

        private static void UpdateLanguageDictionary()
        {
            languageNames = new Dictionary<SystemLanguage, string>();
            var lines = ReadAllLines(LNG_NAMES_FILE);
            for (var i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    languageNames.Add((SystemLanguage) i, lines[i].Trim());
                }
            }

            if (!languageNames.ContainsKey(currentLanguage)) 
                currentLanguage = languageNames.Keys.ElementAt(0);
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

                dict.Add(language,(float) Math.Round((decimal)(100f * filledRowCount / lines.Length), 2));
            }

            return dict;
        }
    }
}