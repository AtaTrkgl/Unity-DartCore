using UnityEngine;
using UnityEditor;


namespace UILib
{
    [CustomEditor(typeof(ProgressBar))]
    public class ProgressBarCustomEditor : Editor
    {
        SerializedProperty min;
        SerializedProperty max;
        SerializedProperty current;
        SerializedProperty fillerColor;
        SerializedProperty isRadial;
        SerializedProperty innerRadius;
        SerializedProperty innerCircleIcon;
        SerializedProperty innerCircleColor;
        SerializedProperty innerCircleIconColor;

        SerializedProperty mask;
        SerializedProperty filler;
        SerializedProperty innerCircle;

        void OnEnable()
        {
            min = serializedObject.FindProperty("min");
            max = serializedObject.FindProperty("max");
            current = serializedObject.FindProperty("current");
            fillerColor = serializedObject.FindProperty("fillerColor");
            isRadial = serializedObject.FindProperty("isRadial");
            innerRadius = serializedObject.FindProperty("innerRadius");
            innerCircleIcon = serializedObject.FindProperty("innerCircleIcon");
            innerCircleColor = serializedObject.FindProperty("innerCircleColor");
            innerCircleIconColor = serializedObject.FindProperty("innerCircleIconColor");

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
            EditorGUILayout.PropertyField(fillerColor, new GUIContent("Filler Color"));
            
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(mask, new GUIContent("Mask"));
            EditorGUILayout.PropertyField(filler, new GUIContent("Filler"));

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(isRadial, new GUIContent("Is Radial"));
            if (isRadial.boolValue)
            {
                EditorGUILayout.PropertyField(innerCircle, new GUIContent("Inner Circle"));
                EditorGUILayout.PropertyField(innerRadius, new GUIContent("Inner Circle Radius"));
                EditorGUILayout.PropertyField(innerCircleColor, new GUIContent("Inner Circle Color"));
                EditorGUILayout.PropertyField(innerCircleIcon, new GUIContent("Inner Circle Icon"));
                EditorGUILayout.PropertyField(innerCircleIconColor, new GUIContent("Inner Circle Icon Color"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
