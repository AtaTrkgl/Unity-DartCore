using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace DartCore.Localization
{
    public class Localizator
    {
        public static string[] keysArray;
        public static string[] currentLanguageArray;

        // Add your language folders' names and their coresponding enums here
        private static Dictionary<SystemLanguage, string> languageNames = new Dictionary<SystemLanguage, string>() {
            { SystemLanguage.English, "en"},{ SystemLanguage.Turkish, "tr"}};

        private static SystemLanguage currentLanguage = SystemLanguage.English;

        private const string lngFilesPath = "Assets/DartCore/Localization/Resources/";
        private const string keysFileName = "_keys";

        public static string GetString(string key, bool returnErrorString = true)
        {
            if (currentLanguageArray == null)
                LoadLanguageFile();
            if (keysArray == null)
                UpdateKeyFile();


            int index = GetIndexOfKey(key);
            bool doesLngFileContainsKey = currentLanguageArray.Length > index;

            if (doesLngFileContainsKey)
            {
                if (index == -1 && returnErrorString)
                    return currentLanguageArray[1];
                else if (index == -1)
                    return "";
                else
                    return currentLanguageArray[index];
            }
            else if (returnErrorString)
                return currentLanguageArray[1];
            else
                return "";
        }


        private static void UpdateKeyFile()
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
                UpdateKeyFile();
            }
        }

        public static void RemoveKey(string key)
        {
            key = key.Replace('\n', new char());
            key = key.Replace(' ', '_');
            if (DoesContainKey(key))
            {
                int index = GetIndexOfKey(key);
                //TODO: Implement
            }
        }

        public static void AddLocalizedValue(string key, string localizedValue, SystemLanguage language)
        {
            if (GetString(key, language, true) != localizedValue)
            {
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
            currentLanguageArray = Resources.Load<TextAsset>(languageNames[currentLanguage]).text.Split('\n');
        }

        public static void UpdateLanguage(SystemLanguage language)
        {
            currentLanguage = language;
            LoadLanguageFile();

            foreach (LanguageUpdater lngUpdater in GameObject.FindObjectsOfType<LanguageUpdater>())
                lngUpdater.UpdateLanguage();
        }

        public static void SetLanguageAccordingToSystem()
        {
            var language = Application.systemLanguage;
            if (languageNames.ContainsKey(language))
                UpdateLanguage(language);
        }

        public static int GetLanguageCount()
        {
            return languageNames.Count;
        }

        public static SystemLanguage[] GetAvailableLanguages()
        {
            SystemLanguage[] languages = new SystemLanguage[languageNames.Keys.Count];
            for (int i = 0; i < languageNames.Count; i++)
                languages[i] = languageNames.Keys.ElementAt(i);

            return languages;
        }

        public static string GetString(string key, SystemLanguage language, bool returnErrorString = true)
        {
            if (keysArray == null)
                UpdateKeyFile();

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
            return keysArray.Contains(key);
        }

        private static int GetIndexOfKey(string key)
        {
            for (int i = 0; i < keysArray.Length; i++)
            {
                if (keysArray[i].Trim() == key)
                    return i;
            }
            return -1;
        }

        public static string[] GetKeys()
        {
            if (keysArray == null)
                UpdateKeyFile();
            return keysArray;
        }
    }
}