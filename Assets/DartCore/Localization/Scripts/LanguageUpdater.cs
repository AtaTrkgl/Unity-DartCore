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
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
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
            text.text = Localizator.GetString(key, returnErrorString:displayErrorMessage);

            if (GetComponent<HyperLink>())
                GetComponent<HyperLink>().UpdateLinkColors();
        }
    }
}