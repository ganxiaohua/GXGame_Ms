using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    [RequireComponent(typeof(Transform))]
    [Serializable]
    public partial class GridData : MonoBehaviour
    {
        public Vector2 CellSize;
        public Vector2Int GirdArea;
        public Vector3 Pos;
        [HideInInspector] public List<Vector2Int> ObstacleCells;
        [HideInInspector] public List<Vector2Int> NoObstacleCells;

        public RectInt GetArea()
        {
            RectInt area = new RectInt(0, 0, GirdArea.x, GirdArea.y);
            return area;
        }

        public Vector3 CellToLocal(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + CellSize.x / 2;
            vector.y = Pos.y;
            vector.z = pos.y * CellSize.y + CellSize.y / 2;
            return vector;
        }

        public Vector3 CellToWolrd(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + Pos.x + CellSize.x / 2;
            vector.y = Pos.y;
            vector.z = pos.y * CellSize.y + Pos.z + CellSize.y / 2;
            return vector;
        }


        public bool InArea(Vector3Int pos)
        {
            RectInt area = GetArea();
            return area.Contains(new Vector2Int(pos.x, pos.z));
        }

        public bool InArea(RectInt rect)
        {
            RectInt area = new RectInt(0, 0, GirdArea.x, GirdArea.y);
            if (rect.xMin >= area.xMin && rect.yMin >= area.yMin && rect.xMax <= area.xMax && rect.yMax <= area.yMax)
            {
                return true;
            }

            return false;
        }

        public Vector3 CellToLocalInterpolated(Vector3 pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x;
            vector.y = Pos.y;
            vector.z = pos.z * CellSize.y;
            return vector;
        }

        public Vector3Int LocalToCell(Vector3 pos)
        {
            Vector3Int vector2Int = new Vector3Int();
            vector2Int.x = Mathf.FloorToInt(pos.x / CellSize.x);
            vector2Int.z = Mathf.FloorToInt(pos.z / CellSize.y);
            return vector2Int;
        }

        public Vector3Int WorldToCell(Vector3 pos)
        {
            Vector3Int v = new Vector3Int();
            pos -= Pos;
            v.x = Mathf.FloorToInt(pos.x / CellSize.x);
            v.z = Mathf.FloorToInt(pos.z / CellSize.y);
            return v;
        }

        public bool IsWorldPosInArea(Vector3 pos)
        {
            var v = WorldToCell(pos);
            return InArea(v);
        }

        public bool AddObstacle(RectInt rect)
        {
            if (!InArea(rect))
            {
                return false;
            }

            if (ObstacleCells == null)
            {
                ObstacleCells = new();
            }

            if (NoObstacleCells == null)
            {
                InitNoObstacleCells();
            }

            for (int x = 0; x < rect.width; x++)
            {
                for (int y = 0; y < rect.height; y++)
                {
                    int posX = rect.x + x;
                    int posY = rect.y + y;
                    var v = new Vector2Int(posX, posY);
                    if (!ObstacleCells.Contains(v))
                    {
                        ObstacleCells.Add(v);
                        NoObstacleCells.RemoveSwapBack(v);
                    }
                }
            }

            return true;
        }

        public void RemoveObstacle(RectInt rect)
        {
            if (ObstacleCells == null)
                return;
            for (int x = 0; x < rect.width; x++)
            {
                for (int y = 0; y < rect.height; y++)
                {
                    int posX = rect.x + x;
                    int posY = rect.y + y;
                    var v = new Vector2Int(posX, posY);
                    ObstacleCells.RemoveSwapBack(v);
                    NoObstacleCells.Add(v);
                }
            }
        }


        public void ClearObstacle()
        {
            ObstacleCells?.Clear();
            InitNoObstacleCells();
        }

        public void InitNoObstacleCells()
        {
            NoObstacleCells ??= new();
            NoObstacleCells.Clear();
            for (int i = 0; i < GirdArea.x; i++)
            {
                for (int j = 0; j < GirdArea.y; j++)
                {
                    NoObstacleCells.Add(new Vector2Int(i, j));
                }
            }
        }
    }
}