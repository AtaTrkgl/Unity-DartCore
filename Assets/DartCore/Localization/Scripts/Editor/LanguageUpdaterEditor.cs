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
            EditorGUILayout.Space();
            
            if (!Localizator.DoesContainKey(lngUpdaterComponent.key))
                EditorGUILayout.HelpBox("The key does not exist in the current context", MessageType.Warning);
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("displayErrorMessage"));
            
            EditorGUILayout.Space();
            if (GUILayout.Button(Localizator.DoesContainKey(lngUpdaterComponent.key) ? $"Edit the '{lngUpdaterComponent.key}' key" 
                : $"Create a key named '{lngUpdaterComponent.key}'"))
            {
                OpenKeyOnEditor();
            }
            
            // If the key was changed
            if (serializedObject.ApplyModifiedProperties())
                lngUpdaterComponent.UpdateLanguage();
        }

        private void OpenKeyOnEditor()
        {
            KeyEditor.key = lngUpdaterComponent.key;
            EditorWindow.FocusWindowIfItsOpen(typeof(KeyEditor));
        }
    }
}

