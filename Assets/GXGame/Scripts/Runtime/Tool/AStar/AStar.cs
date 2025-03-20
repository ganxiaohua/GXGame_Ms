using System;
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
        private int start;
        private int end;
        private bool[] closeList;
        private MinHeap<int> openList;
        private Vector2Int endPos;
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
            closeList = new Boolean[size];
            openList = new MinHeap<int>(size / 2, CompareTo);
        }

        public bool Find(Vector2Int start, Vector2Int end, List<Vector2Int> findPosList)
        {
            Assert.IsTrue(InMap(start) && InMap(end), "发起点不在地图内");
            Assert.IsTrue(start != end, "出发点和起点相同");
            this.start = Pos2Index(start);
            this.end = Pos2Index(end);
            findPosList ??= new();
            findPosList.Clear();
            Array.Clear(closeList, 0, closeList.Length);
            openList.Clear();
            openList.Insert(this.start);
            endPos = end;
            while (openList.Count != 0)
            {
                var index = openList.DeleteMin();
                if (index == this.end)
                {
                    ReCall(findPosList);
                    return true;
                }

                closeList[index] = true;
                CalculateGhAndAddOpenList(index);
            }

            return false;
        }

        private void ReCall(List<Vector2Int> list)
        {
            int cur = end;
            do
            {
                list.Add(Index2Pos(cur));
                cur = parent[cur];
            } while (cur != start);

            list.Reverse();
        }


        private int GetF(int index)
        {
            return g[index] + h[index];
        }

        private void CalculateGhAndAddOpenList(int curIndex)
        {
            var pos = Index2Pos(curIndex);
            for (int i = 0; i < sAroundpos.GetLength(0); i++)
            {
                Vector2Int nextPos = new Vector2Int(pos.x + sAroundpos[i, 0], pos.y + sAroundpos[i, 1]);
                int nextIndex = Pos2Index(nextPos);
                if (!InMap(nextPos) || IsBarrier(nextIndex) || InCloseList(nextIndex))
                {
                    continue;
                }

                bool inOpenList = IsOpenList(nextIndex);
                var nextG = sAroundpos[i, 2] + g[curIndex];
                if (inOpenList && g[nextIndex] > nextG)
                {
                    g[nextIndex] = nextG;
                    parent[nextIndex] = curIndex;
                    openList.Update(nextIndex);
                }
                else if (!inOpenList)
                {
                    g[nextIndex] = nextG;
                    h[nextIndex] = (Mathf.Abs(endPos.x - nextPos.x) + Mathf.Abs(endPos.y - nextPos.y)) * 10;
                    parent[nextIndex] = curIndex;
                    openList.Insert(nextIndex);
                }
            }
        }

        private int CompareTo(int left, int right)
        {
            return GetF(left) - GetF(right);
        }

        private int Pos2Index(Vector2Int pos)
        {
            return pos.y * mapWidth + pos.x;
        }

        private Vector2Int Index2Pos(int index)
        {
            return new Vector2Int(index % mapWidth, index / mapWidth);
        }

        private bool InMap(Vector2Int pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= mapWidth || pos.y >= mapHeight)
            {
                return false;
            }

            return true;
        }


        private bool IsBarrier(int index)
        {
            return barrier[index];
        }

        private bool InCloseList(int index)
        {
            return closeList[index];
        }

        private bool IsOpenList(int index)
        {
            int count = openList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (openList[i] == index)
                {
                    return true;
                }
            }

            return false;
        }
    }
}