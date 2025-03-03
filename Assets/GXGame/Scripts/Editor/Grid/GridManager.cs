using GameFrame;
using UnityEditor;
using UnityEngine;

namespace GXGame.Editor
{
    public partial class GridManager : ScriptableSingleton<GridManager>
    {
        private static GameObject selectGameObject => Selection.activeGameObject != null ? Selection.activeGameObject : null;
        private static GridData GridData => selectGameObject != null ? selectGameObject.GetComponent<GridData>() : null;
        private static Material sMaterial;
        private static Mesh sMesh;
        private static int sLastGridProxyHash;
        private static Vector3 offset = new Vector3(0, 0.01f, 0);
        public bool active => GridData != null;

        private bool registeredEventHandler;

        private static GUIStyle guiStyle;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            instance.RegisterEventHandlers();
        }


        private void OnDisable()
        {
            registeredEventHandler = false;
            SceneView.duringSceneGui -= OnSceneGuiDelegate;
            Selection.selectionChanged -= UpdateCache;
        }

        private void RegisterEventHandlers()
        {
            if (registeredEventHandler)
                return;
            SceneView.duringSceneGui += OnSceneGuiDelegate;
            Selection.selectionChanged += UpdateCache;
            // EditorApplication.hierarchyChanged += UpdateCache;
            // UnityEditor.EditorTools.ToolManager.activeToolChanged += ActiveToolChanged;
            // EditorApplication.quitting += EditorQuitting;
            // GridPaintingState.brushChanged += OnBrushChanged;
            // GridPaintingState.scenePaintTargetChanged += OnScenePaintTargetChanged;
            registeredEventHandler = true;
        }


        private void UpdateCache()
        {
            FlushCachedGridProxy();
            ClearAreaMesh();
            SceneView.RepaintAll();
        }


        static void FlushCachedGridProxy()
        {
            if (sMesh == null)
                return;
            DestroyImmediate(sMesh);
            sMesh = null;
            sMaterial = null;
            guiStyle = null;
        }


        private void OnSceneGuiDelegate(SceneView sceneView)
        {
            if (active && sceneView.drawGizmos)
            {
                int gridHash = GenerateHash(GridData);
                if (sLastGridProxyHash != gridHash)
                {
                    FlushCachedGridProxy();
                    ClearAreaMesh();
                    sLastGridProxyHash = gridHash;
                }

                guiStyle ??= new GUIStyle()
                {
                    normal = new GUIStyleState() {textColor = Color.white},
                    fontSize = 12,
                };
                GetCursorPosWithEditorScene(sceneView);
                BuildPromptMesh(GridData);
                DrawGrid.DrawGridGizmo(GridData,offset, GridData.GridColor, ref sMesh, ref sMaterial);
                Handles.Label(GridData.Pos, $"Grid Info\nCellSize:{GridData.CellSize}\n area:{GridData.GirdArea}", guiStyle);
                Vector3 viewportPoint = new Vector3(50, sceneView.camera.pixelHeight, sceneView.camera.nearClipPlane);
                var pos = sceneView.camera.ScreenToWorldPoint(viewportPoint);
                var sb = StringBuilderCache.Get();
                sb.Append("网络编辑中");
                if (GridData.DrawObstacleZrea)
                    sb.Append(" 镜头锁定了,取消勾选DrawObstacleZrea 解锁");
                Handles.Label(pos, sb.ToString(), guiStyle);
                StringBuilderCache.Release(sb);
            }
        }

        private static int GenerateHash(GridData layout)
        {
            int hash = 0x7ed55d16;
            hash ^= layout.CellSize.GetHashCode();
            hash ^= layout.GirdArea.GetHashCode() << 23;
            hash ^= layout.GridColor.GetHashCode() << 4 + 0x165667b1;
            return hash;
        }
    }
}