using UnityEngine;
using TMPro;

namespace DartCore.Localization
{
    public class LanguageUpdater : MonoBehaviour
    {
        [SerializeField] private string key; 
        private TextMeshProUGUI text;
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            UpdateLanguage();
        }

        public void UpdateLanguage()
        {
            if (text)
                text.text = Localizator.GetString(key);
            else
                text = GetComponent<TextMeshProUGUI>();
        }
    }
}
