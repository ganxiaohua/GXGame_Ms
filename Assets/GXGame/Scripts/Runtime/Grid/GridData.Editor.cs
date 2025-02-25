#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    public partial class GridData
    {
        public Color GridColor = Color.gray;
        [HorizontalGroup("DrawObstacleZrea")] [Title("是否开启障碍物区域绘制")]
        public bool DrawObstacleZrea;

        [HorizontalGroup("DrawObstacleZrea")] [ShowIf("DrawObstacleZrea")] [Title("笔刷大小")]
        public Vector2Int brushSize;
    }
}
#endif