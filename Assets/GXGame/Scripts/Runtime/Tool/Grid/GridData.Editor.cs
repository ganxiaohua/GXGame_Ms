#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    public partial class GridData
    {
        [PropertyOrder(0)] public Color GridColor = Color.gray;

        [VerticalGroup("DrawObstacleZrea")] [Title("是否开启障碍物区域绘制")] [PropertyOrder(1)]
        public bool DrawObstacleZrea;

        [VerticalGroup("DrawObstacleZrea")] [ShowIf("DrawObstacleZrea")] [LabelText("是否开启 清理模式")] [PropertyOrder(1)]
        public bool IsClear;

        [VerticalGroup("DrawObstacleZrea")] [ShowIf("DrawObstacleZrea")] [LabelText("笔刷大小")] [PropertyOrder(1)]
        public Vector2Int brushSize = new Vector2Int(1, 1);

        [VerticalGroup("DrawObstacleZrea")]
        [ShowIf("DrawObstacleZrea")]
        [Button("障碍物点全部清理")]
        [PropertyOrder(1)]
        public void EditorClearObstacle()
        {
            ClearObstacle();
        }

        [VerticalGroup("FindPath")] [Title("是否开启寻路")] [PropertyOrder(2)]
        public bool OpenFindPath;

        [ShowIf("OpenFindPath")] [VerticalGroup("FindPath")] [PropertyOrder(2)]
        public Vector2Int StartPos;

        [ShowIf("OpenFindPath")] [VerticalGroup("FindPath")] [PropertyOrder(2)]
        public Vector2Int EndPos;

        private AStar aStar;

        [ShowIf("OpenFindPath")]
        [VerticalGroup("FindPath")]
        [Button("测试寻路")]
        [PropertyOrder(2)]
        public void FindPathPos()
        {
            if (aStar == null)
            {
                aStar = new AStar();
                aStar.InitFindPath(GirdArea.x, GirdArea.y, obstacleCells);
            }

            bool e = aStar.Find(StartPos, EndPos, FindPath);
            if (!e)
                Debug.Log("无法到达路店");
        }

        [Button("销毁寻路地图")]
        [PropertyOrder(2)]
        public void DestroyFindPath()
        {
            aStar = null;
            FindPath.Clear();
        }

        [HideInInspector] public List<Vector2Int> FindPath;
        [HideInInspector] public Color FindPathColor = Color.cyan;
    }
}
#endif