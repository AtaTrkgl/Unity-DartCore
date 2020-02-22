using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;


namespace UILib
{
    [CustomEditor(typeof(ProgressBar))]
    public class ProgressBarCustomEditor : Editor
    {
        SerializedProperty min;
        SerializedProperty max;
        SerializedProperty current;
        SerializedProperty fillColor;
        SerializedProperty isRadial;
        SerializedProperty innerRadius;
        SerializedProperty innerCircleIcon;
        SerializedProperty innerCircleColor;

        SerializedProperty mask;
        SerializedProperty filler;
        SerializedProperty innerCircle;

        void OnEnable()
        {
            min = serializedObject.FindProperty("min");
            max = serializedObject.FindProperty("max");
            current = serializedObject.FindProperty("current");
            fillColor = serializedObject.FindProperty("fillColor");
            isRadial = serializedObject.FindProperty("isRadial");
            innerRadius = serializedObject.FindProperty("innerRadius");
            innerCircleIcon = serializedObject.FindProperty("innerCircleIcon");
            innerCircleColor = serializedObject.FindProperty("innerCircleColor");

            mask = serializedObject.FindProperty("mask");
            filler = serializedObject.FindProperty("filler");
            innerCircle = serializedObject.FindProperty("innerCircle");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Slider(max, 0, 1000, new GUIContent("Max Value"));
            EditorGUILayout.Slider(min,0 , max.floatValue, new GUIContent("Minimum Value"));
            EditorGUILayout.Slider(current, 0 , max.floatValue, new GUIContent("Current Value"));
            fillColor.colorValue = EditorGUILayout.ColorField("Fill Color", fillColor.colorValue);
            
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(mask, new GUIContent("Mask"));
            EditorGUILayout.PropertyField(filler, new GUIContent("Filler"));

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(isRadial, new GUIContent("Is Radial"));
            if (isRadial.boolValue)
            {
                EditorGUILayout.PropertyField(innerRadius, new GUIContent("Inner Radius"));
                EditorGUILayout.PropertyField(innerCircleIcon, new GUIContent("Inner Circle Icon"));
                EditorGUILayout.PropertyField(innerCircle, new GUIContent("Inner Circle"));
                innerCircleColor.colorValue = EditorGUILayout.ColorField("Inner Circle Color", innerCircleColor.colorValue);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
