using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace DartCore.Utilities
{
    public class EditorScriptingUtils : Editor
    {
        public static void HorizontalLine(int height = 1, float color = .7f)
        {
            color = Mathf.Clamp(color, 0f, 1f);
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;

            EditorGUI.DrawRect(rect, new Color(color, color, color, 1));
        }

        /// <summary>
        /// Must be followed by the EndCenter() function.
        /// </summary>
        public static void BeginCenter()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
        }
        /// <summary>
        /// Must be used after the BeginCenter() function.
        /// </summary>
        public static void EndCenter()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
