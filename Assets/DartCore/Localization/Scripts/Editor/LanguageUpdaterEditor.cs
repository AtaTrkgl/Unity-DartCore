﻿using DartCore.Utilities;
using UnityEditor;
using UnityEngine;

namespace DartCore.Localization.Backend
{
    [CustomEditor(typeof(LanguageUpdater))]
    public class LanguageUpdaterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("key"), new GUIContent("Key"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("displayErrorMessage"), new GUIContent("Display Error Message"));
            EditorScriptingUtils.HorizontalLine();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useFallBackLanguage"), new GUIContent("Use Fallback Language"));
            if (serializedObject.FindProperty("useFallBackLanguage").boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fallbackLanguage"), new GUIContent("Fallback Language"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}