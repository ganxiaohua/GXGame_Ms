#if UNITY_EDITOR
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace GXGame
{
    public partial class FeedbackBoxSystem
    {
        private Vector3 pos;

        private Vector3 size;

        private Quaternion qua = quaternion.identity;

        private GUIStyle guiStyle;

        private void OpenDuringSceneGui()
        {
            guiStyle = new GUIStyle();
            guiStyle.normal.textColor = Color.magenta;
            guiStyle.fontSize = 12;
            guiStyle.padding = new RectOffset(5, 5, 3, 3);
            SceneView.duringSceneGui += OnSceneGuiDelegate;
        }

        private void CloseDuringSceneGui()
        {
            SceneView.duringSceneGui -= OnSceneGuiDelegate;
            SceneView.RepaintAll();
        }

        private void SetPosVector3(Vector3 pos, Quaternion qua, Vector3 size)
        {
            this.pos = pos;
            this.size = size;
            this.qua = qua;
        }

        private void OnSceneGuiDelegate(SceneView view)
        {
            if (!view.drawGizmos)
                return;
            var old = Handles.matrix;
            Handles.matrix = Matrix4x4.TRS(this.pos, qua, Vector3.one);
            Handles.color = Color.red;
            Handles.DrawWireCube(Vector3.zero, this.size);
            Handles.DrawWireCube(Vector3.zero + new Vector3(0.01f, 0, 0), this.size);
            Handles.Label(Vector3.zero, "收集器", guiStyle);
            // Handles.DrawWireCube(Vector3.zero + new Vector3(-0.01f, 0, 0), this.size);
            // Handles.DrawWireCube(Vector3.zero + new Vector3(0, 0.01f, 0), this.size);
            // Handles.DrawWireCube(Vector3.zero + new Vector3(0, -0.01f, 0), this.size);
            // Handles.DrawWireCube(Vector3.zero + new Vector3(0, 0, 0.01f), this.size);
            // Handles.DrawWireCube(Vector3.zero + new Vector3(0, 0, -0.01f), this.size);
            Handles.matrix = old;
        }
    }
}
#endif