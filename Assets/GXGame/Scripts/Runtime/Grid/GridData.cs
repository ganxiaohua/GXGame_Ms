using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    [RequireComponent(typeof(Transform))]
    [ExecuteInEditMode]
    public partial class GridData : MonoBehaviour
    {
        public Vector2 CellSize;
        public RectInt GirdArea;
        public Vector3 Pos;

        [ReadOnly] public List<Vector2Int> ObstacleCell;

        public Vector3 CellToLocal(Vector3Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x;
            vector.y = Pos.y;
            vector.z = pos.z * CellSize.y;
            return vector;
        }

        public Vector3 CellToWolrd(Vector3Int pos)
        {
            var vector = new Vector3();
            vector.x = pos.x * CellSize.x + Pos.x;
            vector.y = Pos.y;
            vector.z = pos.z * CellSize.y + Pos.z;
            return vector;
        }


        public bool InArea(Vector3Int pos)
        {
            return GirdArea.Contains(new Vector2Int(pos.x, pos.z));
        }

        public bool InArea(RectInt rect)
        {
            return GirdArea.Overlaps(rect);
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

            if (ObstacleCell == null)
            {
                ObstacleCell = new();
            }

            for (int x = 0; x < rect.width; x++)
            {
                for (int y = 0; y < rect.height; y++)
                {
                    int posX = rect.x + x;
                    int posY = rect.y + y;
                    var v = new Vector2Int(posX, posY);
                    if (!ObstacleCell.Contains(v))
                        ObstacleCell.Add(v);
                }
            }
            return true;
        }
    }
}