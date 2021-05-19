using UnityEditor;
using UnityEngine;

namespace DartCore.Localization.Backend
{
    [CustomPropertyDrawer(typeof(LocalizedKeyAttribute))]
    public class LocalizedKeyDrawer:PropertyDrawer
    {
        private LocalizedKeyAttribute localizedKeyAttribute => (LocalizedKeyAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var key = Localizator.ConvertRawToKey(property.stringValue);
            var keyExists = DoesKeyExists(key);

            // Rects
            var keyRect = new Rect(position.x, position.y, position.width, 30f);
            var helpBoxRect = new Rect(keyRect.x, keyRect.y + (keyExists ? 0f : 40f), position.width, 35f);
            var editKeyRect = new Rect(keyRect.x, helpBoxRect.y + 40f, position.width, 35f);
            
            // Drawing Property
            EditorGUI.PropertyField(keyRect, property, label);
            property.stringValue = Localizator.ConvertRawToKey(property.stringValue);
            
            if (!keyExists)
                EditorGUI.HelpBox(helpBoxRect, "The key does not exist in the current context", MessageType.Warning);

            if (GUI.Button(editKeyRect, Localizator.DoesContainKey(key) ? $"Edit the '{key}' key" : $"Create a key named '{key}'"))
            {
                OpenKeyOnEditor(key);
            }
        }

        private static bool DoesKeyExists(string key) => Localizator.DoesContainKey(key);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) 
                   + 60f // Button and the margin
                   + (DoesKeyExists(Localizator.ConvertRawToKey(property.stringValue)) ? 0 : 40f); // Help Box
        }
        
        private void OpenKeyOnEditor(string key)
        {
            KeyEditor.key = key;
            EditorWindow.FocusWindowIfItsOpen(typeof(KeyEditor));
        }
    }
}