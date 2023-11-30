using UnityEngine;
using UnityEditor;

namespace Lab5Games.ColorfulHierarchy.Editor
{
    public struct HierarchyWindowItemStates 
    {
        public int InstanceID { get; }
        public HierarchyWindowItem Item { get; }

        public Rect BackgroundRect { get; }
        public Rect TextRect { get; }
        public Rect CollapseToggleIconRect { get; }
        public Rect PrefabIconRect { get; }
        public Rect EditPrefabIconRect;

        public bool IsSelected { get; }
        public bool IsHovered { get; }

        public HierarchyWindowItemStates(int instanceID, Rect selectionRect, HierarchyWindowItem item)
        {
            InstanceID = instanceID;
            Item = item;

            float xPos = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
            float yPos = selectionRect.position.y;
            float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
            float ySize = selectionRect.size.y;
            BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

            xPos = selectionRect.position.x + 18f;
            yPos = selectionRect.position.y;
            xSize = selectionRect.size.x - 18f;
            ySize = selectionRect.size.y;
            TextRect = new Rect(xPos, yPos, xSize, ySize);

            xPos = selectionRect.position.x - 14f;
            yPos = selectionRect.position.y + 1f;
            xSize = 13f;
            ySize = 13f;
            CollapseToggleIconRect = new Rect(xPos, yPos, xSize, ySize);

            xPos = selectionRect.position.x;
            yPos = selectionRect.position.y;
            xSize = 16f;
            ySize = 16f;
            PrefabIconRect = new Rect(xPos, yPos, xSize, ySize);

            xPos = BackgroundRect.xMax - 16f;
            yPos = selectionRect.yMin;
            xSize = 16f;
            ySize = 16f;
            EditPrefabIconRect = new Rect(xPos, yPos, xSize, ySize);

            IsSelected = Selection.Contains(instanceID);
            IsHovered = BackgroundRect.Contains(Event.current.mousePosition);   
        }
    }
}
