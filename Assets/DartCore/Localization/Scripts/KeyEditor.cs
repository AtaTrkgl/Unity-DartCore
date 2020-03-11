using UnityEditor;
using UnityEngine;

namespace DartCore.Localization
{
    public class KeyEditor : EditorWindow
    {
        [MenuItem("DartCore/Localization/Key Editor")]
        public static void OpenWindow()
        {
            var window = new KeyEditor();
            window.titleContent = new GUIContent("Key Editor");
            window.minSize = new Vector2(100, 100);
            window.Show();
        }

        public static string key = "";
        public string keyLastValue;
        public string[] values;

        public void OnGUI()
        {
            var keySearchBarStyle = new GUIStyle(EditorStyles.textField);
            keySearchBarStyle.fixedWidth = position.width * .99f;
            keySearchBarStyle.fixedHeight = 23f;
            keySearchBarStyle.alignment = TextAnchor.MiddleCenter;
            keySearchBarStyle.fontSize = 12;

            key = GUILayout.TextField(key, keySearchBarStyle);
            key = key.Replace(' ','_');
            key = key.ToLower();

            GUILayout.Space(10f);

            if (key != keyLastValue)
                values = new string[Localizator.GetLanguageCount()];

            if (Localizator.DoesContainKey(key))
            {
                GUILayout.BeginScrollView(Vector2.zero, false, true);
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                        values[i] = Localizator.GetString(key, Localizator.GetAvailableLanguages()[i], false);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{Localizator.GetAvailableLanguages()[i]}: ", GUILayout.MaxWidth(50));

                    EditorStyles.textArea.wordWrap = true;
                    values[i] = EditorGUILayout.TextArea(values[i], EditorStyles.textField, GUILayout.Height(position.height * .1f), GUILayout.Width(position.width * .75f));
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
                        $"{key} will be removed",
                        "Are you sure you want to remove this key ?",
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