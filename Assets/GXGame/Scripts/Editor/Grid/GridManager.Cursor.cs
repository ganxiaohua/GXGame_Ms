using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GXGame.Editor
{
    public partial class GridManager
    {
        private Material cursorMaterial;
        private Mesh cursor;
        private Vector3Int curCursorPos;
        private Vector3Int lastCursorPos;
        private bool buttonDown;
        public void GetCursorPosWithEditorScene(SceneView sceneView)
        {
            if (!GridData.DrawObstacleZrea)
            {
                ClearCursor();
                return;
            }

            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPos = ray.GetPoint(enter);
                curCursorPos = GridData.WorldToCell(worldPos);
                Handles.Label(worldPos, $"Cell Position: {curCursorPos}");
                MouseArea();
                InputArea();
            }
        }

        private void MouseArea()
        {
            // if (curCursorPos == lastCursorPos)
            //     return;
            ClearCursor();
            RectInt rect = new RectInt(curCursorPos.x, curCursorPos.z, GridData.brushSize.x, GridData.brushSize.y);
            if (GridData.InArea(rect))
            {
                DrawGrid.CreateGridMesh(GridData, rect, ref cursor, ref cursorMaterial, Color.green);
            }
            // if (Event.current.type == EventType.Repaint)
            // lastCursorPos = curCursorPos;
        }

        private void InputArea()
        {
            var e = Event.current;
            if (e.OnMouseDown(0))
            {
                buttonDown = true;
                cz();
            }else if (e.OnMouseUp(0))
            {
                buttonDown = false;
            }

            if (e.OnMouseMoveDrag() && buttonDown)
            {
                cz();
            }

            void cz()
            {
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                RectInt rect = new RectInt(curCursorPos.x, curCursorPos.z, GridData.brushSize.x, GridData.brushSize.y);
                GridData.AddObstacle(rect);
                Event.current.Use();
                GUIUtility.hotControl = controlID;
            }
        }

        private void ClearCursor()
        {
            if (cursor != null)
            {
                DestroyImmediate(cursor);
                cursor = null;
            }
        }
    }
}