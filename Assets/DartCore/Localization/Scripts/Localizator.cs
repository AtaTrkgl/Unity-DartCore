using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

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

        private const string lngFilesPath = "Assets/DartCore/Localization/Resources/";
        private const string keysFileName = "_keys";
        private const string lngNamesFile = "_lng_names";

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

            if (doesLngFileContainsKey)
                return currentLanguageArray[index];
            else if (returnErrorString)
                return currentLanguageArray[1];
            else
                return "";
        }

        public static void UpdateKeyFile()
        {
            keysArray = Resources.Load<TextAsset>("_keys").text.Split('\n');
        }

        public static void AddKey(string key)
        {
            key = key.Replace('\n', new char());
            key = key.Replace(' ', '_');
            if (!DoesContainKey(key))
            {
                File.AppendAllText(lngFilesPath + keysFileName + ".txt", "\n"+key);
                UnityEditor.AssetDatabase.Refresh();
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
                File.WriteAllText(lngFilesPath + keysFileName + ".txt", newText);

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
                    File.WriteAllText(lngFilesPath + languageNames[language] + ".txt", newText);
                }
            }
            UnityEditor.AssetDatabase.Refresh();
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
                string value = File.ReadAllText(lngFilesPath + languageNames[language] + ".txt");

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

                File.WriteAllText(lngFilesPath + languageNames[language] + ".txt", endString);
                UnityEditor.AssetDatabase.Refresh();
            }
        }

        private static void LoadLanguageFile()
        {
            if (!lngDictIsInitilized)
            {
                UpdateLanguageDictionary();
                lngDictIsInitilized = true;
            }

            currentLanguageArray = Resources.Load<TextAsset>(languageNames[currentLanguage]).text.Split('\n');
        }

        public static bool UpdateLanguage(SystemLanguage language)
        {
            if (!languageNames.ContainsKey(language))
                return false;

            currentLanguage = language;
            LoadLanguageFile();

            foreach (LanguageUpdater lngUpdater in GameObject.FindObjectsOfType<LanguageUpdater>())
                lngUpdater.UpdateLanguage();

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

        public static string GetString(string key, SystemLanguage language, bool returnErrorString = true)
        {
            if (!languageNames.ContainsKey(language))
                return "";

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

        public static bool DoesContainKey(string key)
        {
            UpdateKeyFile();
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

        public static void CreateLanguage(SystemLanguage language, string fileName, string lngName, string lngErrorMessage)
        {
            fileName = fileName.Trim();
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
                File.WriteAllText(lngFilesPath + fileName + ".txt", lngName.Trim() + "\n" + lngErrorMessage.Trim());
                var lines = File.ReadAllText(lngFilesPath + lngNamesFile + ".txt").Split('\n');
                string text = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == (int)language)
                        text += fileName + "\n";
                    else
                        text += lines[i] + "\n";
                }
                text = text.Remove(text.Length - 1);
                File.WriteAllText(lngFilesPath + lngNamesFile + ".txt", text);
                UnityEditor.AssetDatabase.Refresh();
                UpdateLanguageDictionary();
            }
            
        }

        public static void RemoveLanguage(SystemLanguage language)
        {
            if (languageNames.Values.Count == 1)
            {
                Debug.Log("You can not remove the only language available");
                return;
            }

            var lines = File.ReadAllText(lngFilesPath + lngNamesFile + ".txt").Split('\n');
            lines[(int)language] = "";
            string text = "";
            foreach (var line in lines)
                text += line + "\n";
            text = text.Remove(text.Length - 1);
            File.WriteAllText(lngFilesPath + lngNamesFile + ".txt", text);

            File.Delete(lngFilesPath + languageNames[language] + ".txt");

            languageNames.Remove(language);
        }

        private static void UpdateLanguageDictionary()
        {
            languageNames = new Dictionary<SystemLanguage, string>();
            var lines = File.ReadAllText(lngFilesPath + lngNamesFile + ".txt").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                    languageNames.Add((SystemLanguage) i, lines[i].Trim());
            }
            if (!languageNames.ContainsKey(currentLanguage))
            { 
                currentLanguage = languageNames.Keys.ElementAt(0);
                Debug.Log($"Changed the current language to {currentLanguage}");
            }

        }

        public static SystemLanguage GetCurrentLanguage()
        {
            return currentLanguage;
        }
    }
}