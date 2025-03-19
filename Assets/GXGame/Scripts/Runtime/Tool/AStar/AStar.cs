using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class AStar
    {
        private int mapWidth, mapHeight;
        private int[] g;
        private int[] h;
        private bool[] barrier;
        private int[] parent;
        private Vector2Int start;
        private Vector2Int end;
        private HashSet<Vector2Int> closeList;
        private MinHeap<Vector2Int> openList;
        static int[,] sAroundpos = new int[8, 3] {{-1, -1, 14}, {0, -1, 10}, {1, -1, 14}, {1, 0, 10}, {1, 1, 14}, {0, 1, 10}, {-1, 1, 14}, {-1, 0, 10}};

        public void InitFindPath(int width, int height, bool[] barrier)
        {
            mapWidth = width;
            mapHeight = height;
            this.barrier = barrier;
            int size = mapWidth * mapHeight;
            g = new int[size];
            h = new int[size];
            parent = new int[size];
            closeList = new HashSet<Vector2Int>();
            openList = new MinHeap<Vector2Int>(size / 2, CompareTo);
        }

        public bool Find(Vector2Int start, Vector2Int end, List<Vector2Int> findPosList)
        {
            Assert.IsTrue(InMap(start) && InMap(end), "发起点不在地图内");
            Assert.IsTrue(start != end, "出发点和起点相同");
            this.end = end;
            this.start = start;
            findPosList ??= new();
            findPosList.Clear();
            closeList.Clear();
            openList.Clear();
            openList.Insert(start);
            while (openList.Count != 0)
            {
                var pos = openList.DeleteMin();
                if (pos == end)
                {
                    ReCall(findPosList);
                    return true;
                }

                closeList.Add(pos);
                CalculateGhAndAddOpenList(pos);
            }

            return false;
        }

        private void ReCall(List<Vector2Int> list)
        {
            int endIndex = Pos2Index(end);
            int startIndex = Pos2Index(start);
            int cur = endIndex;
            do
            {
                list.Add(new Vector2Int(cur % mapWidth, cur / mapWidth));
                cur = parent[cur];
            } while (cur != startIndex);

            list.Add(start);
            list.Reverse();
        }


        private int GetF(Vector2Int pos)
        {
            int index = Pos2Index(pos);
            return g[index] + h[index];
        }

        private void CalculateGhAndAddOpenList(Vector2Int pos)
        {
            int curIndex = Pos2Index(pos);
            for (int i = 0; i < sAroundpos.GetLength(0); i++)
            {
                Vector2Int nextPos = new Vector2Int(pos.x + sAroundpos[i, 0], pos.y + sAroundpos[i, 1]);
                if (!InMap(nextPos) || IsBarrier(nextPos) || InCloseList(nextPos))
                {
                    continue;
                }

                int nextIndex = Pos2Index(nextPos);
                bool inOpenList = IsOpenList(nextPos);
                var nextG = sAroundpos[i, 2] + g[curIndex];
                if (inOpenList && g[nextIndex] > nextG)
                {
                    g[nextIndex] = nextG;
                    parent[nextIndex] = curIndex;
                    openList.Update(nextPos);
                }
                else if (!inOpenList)
                {
                    g[nextIndex] = nextG;
                    h[nextIndex] = (Mathf.Abs(end.x - nextPos.x) + Mathf.Abs(end.y - nextPos.y)) * 10;
                    parent[nextIndex] = curIndex;
                    openList.Insert(nextPos);
                }
            }
        }

        private int CompareTo(Vector2Int left, Vector2Int right)
        {
            return GetF(left) - GetF(right);
        }

        private int Pos2Index(Vector2Int pos)
        {
            return pos.y * mapWidth + pos.x;
        }

        private bool InMap(Vector2Int pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= mapWidth || pos.y >= mapHeight)
            {
                return false;
            }

            return true;
        }


        private bool IsBarrier(Vector2Int pos)
        {
            int index = Pos2Index(pos);
            return barrier[index];
        }

        private bool InCloseList(Vector2Int pos)
        {
            foreach (var item in closeList)
            {
                if (item == pos)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsOpenList(Vector2Int pos)
        {
            int count = openList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (openList[i] == pos)
                {
                    return true;
                }
            }

            return false;
        }
    }
}