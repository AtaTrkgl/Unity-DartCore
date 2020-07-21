using UnityEditor;
using UnityEngine;

namespace DartCore.Localization
{
    public class KeyEditor : EditorWindow
    {
        [MenuItem("DartCore/Localization/Key Editor")]
        private static void OpenWindow()
        {
            var window = ScriptableObject.CreateInstance<KeyEditor>();
            window.titleContent = new GUIContent("Key Editor");
            window.Show();
        }

        public static string key = "";
        public string keyLastValue;
        public string[] values;
        private Vector2 scrollPos = Vector2.zero;

        private void OnEnable()
        {
            this.minSize = new Vector2(100f, 100f);
        }

        private void OnGUI()
        {
            var keySearchBarStyle = new GUIStyle(EditorStyles.textField)
            {
                fixedWidth = position.width * .99f,
                fixedHeight = 23f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };

            key = GUILayout.TextField(key, keySearchBarStyle);
            key = key.Replace(' ','_').ToLower();

            GUILayout.Space(10f);
            Localizator.UpdateKeyFile();

            if (key != keyLastValue)
                values = new string[Localizator.GetLanguageCount()];

            if (Localizator.DoesContainKey(key))
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                        values[i] = Localizator.GetString(key, Localizator.GetAvailableLanguages()[i], false);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{Localizator.GetAvailableLanguages()[i]}: ", GUILayout.MaxWidth(50));

                    var customTextAreaStyle = EditorStyles.textArea;
                    customTextAreaStyle.wordWrap = true;

                    values[i] = EditorGUILayout.TextArea(
                        values[i], customTextAreaStyle,
                        GUILayout.Height( Mathf.Clamp(((position.height - 97) / values.Length), 100f, float.PositiveInfinity)),
                        GUILayout.Width(position.width * .75f));
                    
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Update Values"))
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        Localizator.AddLocalizedValue(key, values[i], Localizator.GetAvailableLanguages()[i]);
                    }
                }
                if (GUILayout.Button("Remove Key"))
                {
                    bool dialogOutput = EditorUtility.DisplayDialog(
                        $"{key} will be removed permanently",
                        "Are you sure you want to remove this key?",
                        "Remove",
                        "Cancel");
                    if (dialogOutput)
                        Localizator.RemoveKey(key);
                }
            }
            else if (GUILayout.Button("Add New Key") && !string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key))
            {
                Localizator.AddKey(key);
            }

            keyLastValue = key;
        }
    }
}