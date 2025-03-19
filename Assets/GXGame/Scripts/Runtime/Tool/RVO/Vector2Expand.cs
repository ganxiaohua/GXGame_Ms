using UnityEngine;

namespace GXGame.Scripts.Runtime.RVO
{
    public static class Vector2Expand
    {
        public static float X(this Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y;
        }
    }
}