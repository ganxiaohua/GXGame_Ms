using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    [RequireComponent(typeof(Transform))]
    [Serializable]
    public partial class GridData : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private Vector2 cellSize;
        [HideInInspector] [SerializeField] private Vector2Int girdArea;
        [HideInInspector] [SerializeField] private Vector3 pos;
        [HideInInspector] [SerializeField] private bool[] obstacleCells;

        [ShowInInspector]
        [PropertyOrder(-1)]
        [VerticalGroup("基础数据")]
        public Vector2 CellSize
        {
            get => cellSize;
            private set => cellSize = value;
        }

        [ShowInInspector]
        [PropertyOrder(-1)]
        [VerticalGroup("基础数据")]
        public Vector2Int GirdArea
        {
            get => girdArea;
            private set
            {
                girdArea = value;
                obstacleCells = new bool[girdArea.x * girdArea.y];
            }
        }

        [ShowInInspector]
        [PropertyOrder(-1)]
        [VerticalGroup("基础数据")]
        public Vector3 Pos
        {
            get => pos;
            private set => pos = value;
        }

        public bool[] ObstacleCells
        {
            get => obstacleCells;
            private set => obstacleCells = value;
        }


        public RectInt GetArea()
        {
            RectInt area = new RectInt(0, 0, girdArea.x, girdArea.y);
            return area;
        }

        public Vector3 CellToLocal(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * cellSize.x + cellSize.x / 2;
            vector.y = this.pos.y;
            vector.z = pos.y * cellSize.y + cellSize.y / 2;
            return vector;
        }

        public Vector3 CellToWolrd(Vector2Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * cellSize.x + this.pos.x + cellSize.x / 2;
            vector.y = this.pos.y;
            vector.z = pos.y * cellSize.y + this.pos.z + cellSize.y / 2;
            return vector;
        }


        public bool InArea(Vector3Int pos)
        {
            RectInt area = GetArea();
            return area.Contains(new Vector2Int(pos.x, pos.z));
        }

        public bool InArea(RectInt rect)
        {
            RectInt area = new RectInt(0, 0, girdArea.x, girdArea.y);
            if (rect.xMin >= area.xMin && rect.yMin >= area.yMin && rect.xMax <= area.xMax && rect.yMax <= area.yMax)
            {
                return true;
            }

            return false;
        }

        public Vector3 CellToLocalInterpolated(Vector3 pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * cellSize.x;
            vector.y = this.pos.y;
            vector.z = pos.z * cellSize.y;
            return vector;
        }

        public Vector3Int LocalToCell(Vector3 pos)
        {
            Vector3Int vector2Int = new Vector3Int();
            vector2Int.x = Mathf.FloorToInt(pos.x / cellSize.x);
            vector2Int.z = Mathf.FloorToInt(pos.z / cellSize.y);
            return vector2Int;
        }

        public Vector3Int WorldToCell(Vector3 pos)
        {
            Vector3Int v = new Vector3Int();
            pos -= this.pos;
            v.x = Mathf.FloorToInt(pos.x / cellSize.x);
            v.z = Mathf.FloorToInt(pos.z / cellSize.y);
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


            for (int x = 0; x < rect.width; x++)

            {
                for (int y = 0; y < rect.height; y++)
                {
                    int posX = rect.x + x;
                    int posY = rect.y + y;
                    var v = new Vector2Int(posX, posY);
                    obstacleCells[posY * girdArea.x + posX] = true;
                }
            }

            return true;
        }

        public void RemoveObstacle(RectInt rect)
        {
            if (obstacleCells == null)
                return;
            for (int x = 0; x < rect.width; x++)
            {
                for (int y = 0; y < rect.height; y++)
                {
                    int posX = rect.x + x;
                    int posY = rect.y + y;
                    var v = new Vector2Int(posX, posY);
                    obstacleCells[posY * girdArea.x + posX] = false;
                }
            }
        }

        public void ClearObstacle()
        {
            for (int x = 0; x < girdArea.x; x++)
            {
                for (int y = 0; y < girdArea.y; y++)
                {
                    obstacleCells[y * girdArea.x + x] = false;
                }
            }
        }
    }
}