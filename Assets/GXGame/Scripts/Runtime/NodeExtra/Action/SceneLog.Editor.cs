#if UNITY_EDITOR
using ParadoxNotion;
using UnityEngine;

namespace GXGame
{
    public partial class SceneLog
    {
        void OnGUI()
        {
            if (Camera.main == null)
            {
                return;
            }

            var pos = owner.GetWorldPos().Value;
            var point = Camera.main.WorldToScreenPoint(pos);
            var size = GUI.skin.label.CalcSize(new GUIContent(LOG.value));
            var r = new Rect(point.x - size.x / 2, Screen.height - point.y, size.x + 10, size.y);
            GUI.color = Color.white.WithAlpha(0.5f);
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
            r.x += 4;
            GUI.Label(r, LOG.value);
            GUI.color = Color.white;
        }
    }
}
#endif