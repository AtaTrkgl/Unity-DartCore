using UnityEngine;
using UnityEditor;


namespace UILib
{
    [CustomEditor(typeof(ProgressBar))]
    public class ProgressBarCustomEditor : Editor
    {
        SerializedProperty max;
        SerializedProperty isRadial;

        void OnEnable()
        {
            max = serializedObject.FindProperty("max");
            isRadial = serializedObject.FindProperty("isRadial");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("hasBoundries"), new GUIContent("Has Boundries"));
            EditorGUILayout.Slider(max, 0, 1000, new GUIContent("Max Value"));
            EditorGUILayout.Slider(serializedObject.FindProperty("min"), 0 , max.floatValue, new GUIContent("Minimum Value"));
            EditorGUILayout.Slider(serializedObject.FindProperty("current"), 0 , max.floatValue, new GUIContent("Current Value"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fillerColor"), new GUIContent("Filler Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fillSmoothly"), new GUIContent("Fill Smoothly"));
            
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("mask"), new GUIContent("Mask"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("filler"), new GUIContent("Filler"));

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(isRadial, new GUIContent("Is Radial"));
            if (isRadial.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("innerCircle"), new GUIContent("Inner Circle"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("innerRadius"), new GUIContent("Inner Circle Radius"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("innerCircleColor"), new GUIContent("Inner Circle Color"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("innerCircleIcon"), new GUIContent("Inner Circle Icon"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("innerCircleIconColor"), new GUIContent("Inner Circle Icon Color"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
