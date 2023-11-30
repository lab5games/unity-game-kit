using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Lab5Games.ColorfulHierarchy
{
    [System.Serializable]
    public class FontWrapper
    {
        public Font font;
        public int size;
        public FontStyle style;
        public TextAnchor alignment;
    }

    [DisallowMultipleComponent]
    public class HierarchyWindowItem : MonoBehaviour
    {
        // background
        public bool UseDefaultBackgroundColor;
        public Color32 BackgroundColor = new Color32(0, 0, 0, 255);
        // text
        public TextAnchor TextAnchor = TextAnchor.MiddleCenter;
        public bool TextDropShadow;
        public bool UseDefaultTextColor;
        public Color32 TextColor = new Color32(255, 255, 255, 255);
        // font
        public Font Font;
        public int FontSize = 12;
        public FontStyle FontStyle = FontStyle.Normal;
        


#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }
#endif
    }
}
