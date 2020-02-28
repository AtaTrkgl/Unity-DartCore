using System.Collections.Generic;
using UnityEngine;
using System;

namespace DartCore.Localization
{
    public class Localizator
    {
        public static TextAsset csvFile;
        private static char lineSeperator = '\n';
        private static char fieldSeperator = ',';

        private static Language currentLanguage = Language.English;

        private static Dictionary<Language, string> languageCodes = 
            new Dictionary<Language, string>(){ 
            { Language.English , "en" }, { Language.Turkish, "tr"},
            { Language.French , "fr" }, { Language.German, "de"},
            };

        private static Dictionary<string, string> currentLanguageFile;

        public static string GetString(string id)
        {
            if (currentLanguageFile == null)
                UpdateLanguage(currentLanguage);
            if (currentLanguageFile == null)
                return "Language Error";

            if (currentLanguageFile.ContainsKey(id))
                return currentLanguageFile[id];
            else
                return "Language Error";
        }

        private static void LoadCSV()
        {
            csvFile = Resources.Load<TextAsset>("language_file");
        }

        public static void UpdateLanguage(Language language)
        {
            if (csvFile == null)
                LoadCSV();

            currentLanguage = language;
            var lines = csvFile.text.Split(lineSeperator);

            var firstLine = lines[0].Split(fieldSeperator);
            int index = -1;
            for (int i = 0; i < firstLine.Length; i++)
            {
                if (firstLine[i] == languageCodes[currentLanguage])
                { 
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                Debug.LogError($"{currentLanguage} NOT FOUND");
                return;
            }

            currentLanguageFile = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                var splittedLine = line.Split(fieldSeperator);
                currentLanguageFile.Add(splittedLine[0], splittedLine[index]);
            }

            foreach (var lngUpdater in GameObject.FindObjectsOfType<LanguageUpdater>())
                lngUpdater.UpdateLanguage();
        }
    }

    public enum Language : byte
    { 
        English = 0,
        Turkish = 1,
        French = 2,
        German = 3
    }
}