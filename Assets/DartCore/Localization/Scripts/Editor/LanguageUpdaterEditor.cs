using System;
using System.Linq;
using ICSharpCode.NRefactory.PrettyPrinter;
using UnityEngine;
using UnityEditor;

namespace DartCore.Localization
{
    [CustomEditor(typeof(LanguageUpdater))]
    public class LanguageUpdaterEditor : Editor
    {
        private LanguageUpdater lngUpdaterComponent;
        private void OnEnable()
        {
            lngUpdaterComponent = (LanguageUpdater) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));
            
            // If the key was changed
            if (serializedObject.ApplyModifiedProperties())
                lngUpdaterComponent.UpdateLanguage();

            var keyIsValid = !string.IsNullOrWhiteSpace(Localizator.GetString(lngUpdaterComponent.key, false));
            if (!keyIsValid)
                EditorGUILayout.HelpBox("The key does not exist in the current context", MessageType.Warning);
            
            if (GUILayout.Button(keyIsValid ? $"Edit the '{lngUpdaterComponent.key}' key" 
                : $"Create a key named '{lngUpdaterComponent.key}'"))
            {
                OpenKeyOnEditor();
            }
        }

        private void OpenKeyOnEditor()
        {
            KeyEditor.key = lngUpdaterComponent.key;
            EditorWindow.FocusWindowIfItsOpen(typeof(KeyEditor));
        }
    }
}

