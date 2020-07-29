using UnityEngine;
using TMPro;
using DartCore.UI;

namespace DartCore.Localization
{
    [ExecuteInEditMode]
    public class LanguageUpdater : MonoBehaviour
    {
        public string key;
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
            text.text = Localizator.GetString(key);

            if (GetComponent<HyperLink>())
                GetComponent<HyperLink>().UpdateLinkColors();
        }
    }
}