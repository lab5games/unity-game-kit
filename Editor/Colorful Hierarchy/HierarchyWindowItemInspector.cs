using UnityEngine;
using UnityEditor;

namespace Lab5Games.ColorfulHierarchy.Editor
{
    [CustomEditor(typeof(HierarchyWindowItem))]
    public class HierarchyWindowItemInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // background
            EditorGUILayout.LabelField("Background", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            
            var useDefaultBackgroundColor = serializedObject.FindProperty("UseDefaultBackgroundColor");
            EditorGUILayout.PropertyField(useDefaultBackgroundColor, new GUIContent("Use Default Color"));
            
            if(!useDefaultBackgroundColor.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BackgroundColor"), new GUIContent("Color"));
            }

            // text
            //EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            --EditorGUI.indentLevel;
            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("TextAnchor"), new GUIContent("Anchor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TextDropShadow"), new
                GUIContent("Drop Shadow"));

            var useDefaultTextColor = serializedObject.FindProperty("UseDefaultTextColor");
            EditorGUILayout.PropertyField(useDefaultTextColor, new GUIContent("Use Default Color"));

            if(!useDefaultTextColor.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TextColor"), new GUIContent("Color"));
            }

            // font
            //EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
            --EditorGUI.indentLevel;
            EditorGUILayout.LabelField("Font", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Font"), new GUIContent("Font"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FontSize"), new GUIContent("Size"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FontStyle"), new GUIContent("Style"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}