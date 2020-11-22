using System;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace  DartCore.UI
{
    public class FPSDisplayer : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/FPS Displayer"), MenuItem("GameObject/UI/DartCore/FPS Displayer")]
        public static void AddVersionDisplayer()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("FPS Displayer"),
                Selection.activeGameObject ? Selection.activeGameObject.transform : null,
                false);
            obj.name = "FPS Displayer";
        }
#endif

        #endregion

        public static FPSDisplayer instance;

        [SerializeField] private Color textColor = Color.green;
        private TMP_Text versionText;
    
        private void Awake()
        {
            if (instance)
                Destroy(gameObject);
            else
                instance = this;
            
            versionText = GetComponentInChildren<TMP_Text>();
            
            DontDestroyOnLoad(this);
            UpdateColor(textColor);
        }

        private void Update()
        {
            if (!versionText || Time.unscaledDeltaTime == 0f) return;
            versionText.text = $"FPS: {Mathf.RoundToInt(1/Time.unscaledDeltaTime)}";
        }

        public void UpdateColor(Color color)
        {
            textColor = color;
            versionText.color = color;
        }
    }
}