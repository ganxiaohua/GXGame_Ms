using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace GXGame
{
    public static partial class CollisionDetection
    {
        private static Vector3 epos;

        private static Vector3 esize;

        private static Quaternion equa = quaternion.identity;

        private static GUIStyle eguiStyle;

        public static void Init()
        {
            eguiStyle = new GUIStyle();
            eguiStyle.normal.textColor = Color.magenta;
            eguiStyle.fontSize = 12;
            eguiStyle.padding = new RectOffset(5, 5, 3, 3);
            SceneView.duringSceneGui += OnSceneGuiDelegate;
        }

        public static void Clear()
        {
            SceneView.duringSceneGui -= OnSceneGuiDelegate;
            SceneView.RepaintAll();
        }

        private static void SetBox(Vector3 pos, Quaternion qua, Vector3 size)
        {
            CollisionDetection.epos = pos;
            CollisionDetection.esize = size;
            CollisionDetection.equa = qua;
        }

        private static void OnSceneGuiDelegate(SceneView view)
        {
            if (!view.drawGizmos)
                return;
            var old = Handles.matrix;
            Handles.matrix = Matrix4x4.TRS(epos, equa, Vector3.one);
            Handles.color = Color.red;
            Handles.DrawWireCube(Vector3.zero, esize);
            Handles.DrawWireCube(Vector3.zero + new Vector3(0.01f, 0, 0), esize);
            Handles.Label(Vector3.zero, "收集器", eguiStyle);
            Handles.matrix = old;
        }
    }
}
#endif