using UnityEngine;
using TMPro;
using DartCore.UI;

namespace DartCore.Localization
{
    [ExecuteInEditMode]
    public class LanguageUpdater : MonoBehaviour
    {
        [LocalizedKey] public string key;
        [Tooltip("Should an error message get displayed when there is no value for the given key, if set to false will just remain empty.")]
        public bool displayErrorMessage = true;
        public bool useFallBackLanguage = true;
        public SystemLanguage fallbackLanguage = SystemLanguage.English;

        public string prefix;
        public string suffix;
        
        private TextMeshProUGUI text;
        private HyperLink hyperLink;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            hyperLink = GetComponent<HyperLink>();
        }

        private void OnEnable()
        {
            UpdateLanguage();
            Localizator.OnLanguageChange += UpdateLanguage;
        }

        private void OnDisable()
        {
            Localizator.OnLanguageChange -= UpdateLanguage;
        }

        public void UpdateLanguage()
        {
            if (!text)
                text = GetComponent<TextMeshProUGUI>();

            var localizedText = !useFallBackLanguage
                ? Localizator.GetString(key, displayErrorMessage)
                : Localizator.GetStringWithFallBackLanguage(key, fallbackLanguage, displayErrorMessage);
            
            text.text = prefix + localizedText + suffix;

            if (!hyperLink)
                hyperLink = GetComponent<HyperLink>();
            
            if (hyperLink)
                hyperLink.UpdateLinkColors();
        }
    }
}