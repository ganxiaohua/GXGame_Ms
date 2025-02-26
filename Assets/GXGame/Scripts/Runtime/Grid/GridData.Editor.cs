#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    public partial class GridData
    {
        public Color GridColor = Color.gray;

        [HorizontalGroup("DrawObstacleZrea")] [Title("是否开启障碍物区域绘制")]
        public bool DrawObstacleZrea;

        [HorizontalGroup("DrawObstacleZrea")] [ShowIf("DrawObstacleZrea")] [Title("是否开启 清理模式")]
        public bool IsClear;

        [HorizontalGroup("DrawObstacleZrea")] [ShowIf("DrawObstacleZrea")] [Title("笔刷大小")]
        public Vector2Int brushSize = new Vector2Int(1, 1);

        [HideInInspector] public List<Vector2Int> FindPath;
        [HideInInspector] public Color FindPathColor = Color.cyan;

        [ShowIf("DrawObstacleZrea")]
        [Button("障碍物点全部清理")]
        public void EditorClearObstacle()
        {
            ClearObstacle();
        }
        
        
    }
}
#endif