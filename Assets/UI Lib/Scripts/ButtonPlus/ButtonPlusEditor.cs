using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UILib
{
    [CustomEditor(typeof(ButtonPlus))]
    public class ButtonPlusEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("toolTip"), new GUIContent("Tooltip Text"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("textColor"), new GUIContent("Tooltip Text Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bgColor"), new GUIContent("Tooltip BG Color"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}