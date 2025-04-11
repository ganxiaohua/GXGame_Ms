using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace GXGame
{
    public static partial class CollisionDetection
    {
        private static Vector3 pos;

        private static Vector3 size;

        private static Quaternion qua = quaternion.identity;

        private static GUIStyle guiStyle;

        public static void Init()
        {
            guiStyle = new GUIStyle();
            guiStyle.normal.textColor = Color.magenta;
            guiStyle.fontSize = 12;
            guiStyle.padding = new RectOffset(5, 5, 3, 3);
            SceneView.duringSceneGui += OnSceneGuiDelegate;
        }

        public static void Clear()
        {
            SceneView.duringSceneGui -= OnSceneGuiDelegate;
            SceneView.RepaintAll();
        }

        private static void SetBox(Vector3 pos, Quaternion qua, Vector3 size)
        {
            CollisionDetection.pos = pos;
            CollisionDetection.size = size;
            CollisionDetection.qua = qua;
        }

        private static void OnSceneGuiDelegate(SceneView view)
        {
            if (!view.drawGizmos)
                return;
            var old = Handles.matrix;
            Handles.matrix = Matrix4x4.TRS(pos, qua, Vector3.one);
            Handles.color = Color.red;
            Handles.DrawWireCube(Vector3.zero, size);
            Handles.DrawWireCube(Vector3.zero + new Vector3(0.01f, 0, 0), size);
            Handles.Label(Vector3.zero, "收集器", guiStyle);
            Handles.matrix = old;
        }
    }
}
#endif