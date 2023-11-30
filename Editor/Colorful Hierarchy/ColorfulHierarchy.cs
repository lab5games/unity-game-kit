using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

// https://github.com/NCEEGEE/PrettyHierarchy/blob/main/Editor/PrettyHierarchy.cs

namespace Lab5Games.ColorfulHierarchy.Editor
{
    [InitializeOnLoad]
    public static class ColorfulHierarchy 
    {
        static ColorfulHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
        }


        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);

            if(instance != null)
            {
                HierarchyWindowItem item = (instance as GameObject).GetComponent<HierarchyWindowItem>();

                if(item != null)
                {
                    HierarchyWindowItemStates states = new HierarchyWindowItemStates(instanceID, selectionRect, item);

                    DrawBackground(states);
                    DrawText(states);
                    DrawCollapseToggleIcon(states);
                    DrawPrefabIcon(states);
                    DrawEditPrefabIcon(states); 
                }
            }
        }

        static void DrawBackground(HierarchyWindowItemStates states)
        {
            Color32 color;
            if (states.IsHovered && !states.IsSelected)
            {
                color = ColorSettings.HoverOverlay;
            }
            else
            {
                if (states.Item.UseDefaultBackgroundColor || states.IsSelected)
                {
                    color = ColorSettings.GetDefaultBackgroundColor(EditorUtils.IsHierarchyFocused, states.IsSelected);
                }
                else
                {
                    color = states.Item.BackgroundColor;
                }
            }

            EditorGUI.DrawRect(states.BackgroundRect, color);
        }

        static void DrawText(HierarchyWindowItemStates states)
        {
            Color32 color;
            if (states.IsHovered && !states.IsSelected)
            {
                
            }
            else
            {
                if (states.Item.UseDefaultTextColor || states.IsSelected)
                {
                    color = ColorSettings.GetDefaultTextColor(EditorUtils.IsHierarchyFocused, states.IsSelected, states.Item.gameObject.activeInHierarchy);
                }
                else
                {
                    color = states.Item.TextColor;
                    color.a = states.Item.gameObject.activeInHierarchy ? ColorSettings.TextAlphaObjectEnabled : ColorSettings.TextAlphaObjectDisabled;
                }

                GUIStyle newStyle = new GUIStyle
                {
                    normal = new GUIStyleState() { textColor = color },
                    alignment = states.Item.TextAnchor,
                    fontStyle = states.Item.FontStyle,
                    font = states.Item.Font,
                    fontSize = states.Item.FontSize
                };

                if (states.Item.TextDropShadow)
                {
                    EditorGUI.DropShadowLabel(states.TextRect, states.Item.gameObject.name, newStyle);
                }
                else
                {
                    EditorGUI.LabelField(states.TextRect, states.Item.gameObject.name, newStyle);
                }
            }
        }

        static void DrawCollapseToggleIcon(HierarchyWindowItemStates states)
        {
            if(states.Item.transform.childCount > 0)
            {
                Type sceneHierarchyWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
                PropertyInfo sceneHierarchyWindow = sceneHierarchyWindowType.GetProperty("lastInteractedHierarchyWindow", BindingFlags.Public | BindingFlags.Static);

                int[] expandedIDs = (int[])sceneHierarchyWindowType.GetMethod("GetExpandedIDs", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(sceneHierarchyWindow.GetValue(null), null);

                string iconID = expandedIDs.Contains(states.InstanceID) ? "IN Foldout on" : "IN foldout";

                GUI.DrawTexture(states.CollapseToggleIconRect, EditorGUIUtility.IconContent(iconID).image, ScaleMode.StretchToFill, true, 0f, ColorSettings.CollapseIconTintColor, 0f, 0f);
            }
        }

        static void DrawPrefabIcon(HierarchyWindowItemStates states)
        {
            Texture icon = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(states.InstanceID), null).image;

            // The above does not account for the selection highlight, so we do it manually...
            if (EditorUtils.IsHierarchyFocused && states.IsSelected)
            {
                if (icon.name == "d_Prefab Icon" || icon.name == "Prefab Icon")
                {
                    icon = EditorGUIUtility.IconContent("d_Prefab On Icon").image;
                }
                else if (icon.name == "GameObject Icon") // Dark theme is fine by default here...
                {
                    icon = EditorGUIUtility.IconContent("GameObject On Icon").image;
                }
            }

            // Alpha of the icon is affected by the object's active/inactive state
            Color color = states.Item.gameObject.activeInHierarchy ? Color.white : new Color(1f, 1f, 1f, 0.5f);

            //GUI.DrawTexture(item.PrefabIconRect, tex, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
            GUI.DrawTexture(states.PrefabIconRect, icon, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
        }

        static void DrawEditPrefabIcon(HierarchyWindowItemStates states)
        {
            if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(states.Item.gameObject) != null && PrefabUtility.IsAnyPrefabInstanceRoot(states.Item.gameObject))
            {
                Texture icon = EditorGUIUtility.IconContent("ArrowNavigationRight").image;
                GUI.DrawTexture(states.EditPrefabIconRect, icon, ScaleMode.StretchToFill, true, 0f, ColorSettings.EditPrefabIconTintColor, 0f, 0f);
            }
        }
    }
}
