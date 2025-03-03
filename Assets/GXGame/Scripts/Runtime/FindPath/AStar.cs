using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using GXGame;
using UnityEngine;

public struct AStarData
{
    public int F
    {
        get { return G + H; }
    }

    public int G { get; set; }
    public int H { get; set; }

    public AStarData(int g, int h)
    {
        G = g;
        H = h;
    }
}

public class AStarGrid
{
    public int X { get; set; }
    public int Y { get; set; }
    public AStarData Data;
    public AStarGrid Parent;

    public AStarGrid(int x, int y)
    {
        X = x;
        Y = y;
    }

    public AStarGrid(int x, int y, int g, int h)
    {
        X = x;
        Y = y;
        Data = new AStarData(g, h);
    }
}

public class Astar : IVersions, IDisposable
{
    public int Versions { get; set; }
    List<AStarGrid> mOpenlist = new List<AStarGrid>();

    List<AStarGrid> mCloselist = new List<AStarGrid>();

    /// <summary>
    /// 周围格子的属性
    /// </summary>
    static int[,] Aroundpos = new int[8, 3] {{-1, -1, 14}, {0, -1, 10}, {1, -1, 14}, {1, 0, 10}, {1, 1, 14}, {0, 1, 10}, {-1, 1, 14}, {-1, 0, 10}};

    private const int FrameMaxFind = 15;

    List<AStarGrid> Barrierlist = new List<AStarGrid>();


    private int CurFrameMaxFind = 0;

    /// <summary>
    /// 全格子列表
    /// </summary>
    public List<AStarGrid> Maps = new List<AStarGrid>();

    private int[] MapWithHeight = new int[2];

    private List<Vector2Int> finalPath;

    /// <summary>
    /// 设置地图
    /// </summary>
    public void InitMap(GridData gridData)
    {
        Maps.Clear();
        MapWithHeight[0] = gridData.GirdArea.x; //列
        MapWithHeight[1] = gridData.GirdArea.y; //行

        for (int x = 0; x < MapWithHeight[0]; x++)
        {
            for (int y = 0; y < MapWithHeight[1]; y++)
            {
                var AStarGrid = new AStarGrid(x, y);
                Maps.Add(AStarGrid);
            }
        }

        SetBarrier(gridData.ObstacleCells);
    }

    public void SetBarrier(List<Vector2Int> barrier)
    {
        Barrierlist.Clear();
        foreach (var item in barrier)
        {
            Barrierlist.Add(new AStarGrid(item.x, item.y));
        }
    }

    bool InMap(int x, int y)
    {
        if (x >= 0 && x < MapWithHeight[0] && y >= 0 && y < MapWithHeight[1])
        {
            return true;
        }

        return false;
    }


    AStarGrid GetGoToGrid()
    {
        AStarGrid grid = mOpenlist[0];
        int min = grid.Data.F;
        for (int i = 1; i < mOpenlist.Count; i++)
        {
            var data = mOpenlist[i].Data;
            if (data.F < min)
            {
                min = data.F;
                grid = mOpenlist[i];
            }
        }

        return grid;
    }

    AStarGrid GetGridWithList(int x, int y, List<AStarGrid> list)
    {
        foreach (var item in list)
        {
            if (x == item.X && y == item.Y)
                return item;
        }

        return new AStarGrid(0, 0);
    }

    private int GetHValue(int x, int y, AStarGrid endgrid)
    {
        int xcount = Mathf.Abs(endgrid.X - x);
        int ycount = Mathf.Abs(endgrid.Y - y);
        int Hvalue = (xcount + ycount) * 10;
        return Hvalue;
    }

    private AStarGrid GetHMinBarrier(int x, int y)
    {
        AStarGrid mingrid = Barrierlist[0];
        int min = 0;
        foreach (var item in Barrierlist)
        {
            int curvalue = GetHValue(x, y, item);
            if (min == 0 || curvalue < min)
            {
                min = curvalue;
                mingrid = item;
            }
        }

        return mingrid;
    }

    private bool IsInList(int x, int y, List<AStarGrid> list)
    {
        foreach (var item in list)
        {
            if (x == item.X && y == item.Y)
                return true;
        }

        return false;
    }

    bool CanMoveGrid(AStarGrid grid)
    {
        if (grid.Data.G == 14 && Barrierlist.Count > 0)
        {
            var minbarrier = GetHMinBarrier(grid.X, grid.Y);
            if ((minbarrier.X + 1 == grid.X || minbarrier.X - 1 == grid.X) || (minbarrier.Y + 1 == grid.Y || minbarrier.Y - 1 == grid.Y))
                return false;
        }

        return true;
    }

    void AddAroundGrid(AStarGrid curgrid, AStarGrid endgrid)
    {
        for (int i = 0; i < Aroundpos.GetLength(0); i++)
        {
            var x = curgrid.X + Aroundpos[i, 0];
            var y = curgrid.Y + Aroundpos[i, 1];
            //可以优化
            if (InMap(x, y) && !IsInList(x, y, mCloselist) && !IsInList(x, y, Barrierlist))
            {
                if (IsInList(x, y, mOpenlist))
                {
                    var grid = GetGridWithList(x, y, mOpenlist);
                    grid.Data.G = Aroundpos[i, 2];
                }
                else
                {
                    int gvalue = Aroundpos[i, 2];
                    int hvalue = GetHValue(x, y, endgrid);
                    var grid = new AStarGrid(x, y, gvalue, hvalue);
                    if (CanMoveGrid(grid))
                    {
                        grid.Parent = curgrid;
                        mOpenlist.Add(grid);
                    }
                }
            }
        }
    }

    public AStarGrid CreatStart(int x, int y)
    {
        AStarGrid grid = new AStarGrid(x, y, 0, 0);
        mOpenlist.Add(grid);
        return grid;
    }

    public AStarGrid CreatEnd(int x, int y)
    {
        AStarGrid grid = new AStarGrid(x, y, 0, 0);
        return grid;
    }

    private async UniTask<List<Vector2Int>> StartAStarArithmetic(AStarGrid start, AStarGrid end)
    {
        if (!InMap(start.X, start.Y) || !InMap(end.X, end.Y))
        {
            Debug.LogWarning("寻路位置不在地图内.");
            return null;
        }

        CurFrameMaxFind = 0;
        while (mOpenlist.Count > 0)
        {
            var gotogrid = GetGoToGrid();
            if (gotogrid.X == end.X && gotogrid.Y == end.Y)
            {
                end.Parent = gotogrid.Parent;
                break;
            }

            mOpenlist.Remove(gotogrid);
            if (!mCloselist.Contains(gotogrid))
                mCloselist.Add(gotogrid);
            AddAroundGrid(gotogrid, end);
            CurFrameMaxFind++;
            if (CurFrameMaxFind >= FrameMaxFind)
            {
                await UniTask.Yield();
                CurFrameMaxFind = 0;
            }
        }

        AStarGrid endgrid = end;

        while (endgrid.Parent != null)
        {
            finalPath.Add(new Vector2Int(endgrid.X, endgrid.Y));
            endgrid = endgrid.Parent;
        }

        if (finalPath.Count > 0)
        {
            finalPath.RemoveAt(finalPath.Count - 1);
            finalPath.Reverse();
        }

        return finalPath;
    }


    public async UniTask<List<Vector2Int>> Find(Vector2Int startpos, Vector2Int endpos, List<Vector2Int> finalPath)
    {
        finalPath ??= new();
        finalPath.Clear();
        this.finalPath = finalPath;
        Versions++;
        mOpenlist.Clear();
        mCloselist.Clear();
        var start = CreatStart(startpos[0], startpos[1]);
        var end = CreatEnd(endpos[0], endpos[1]);
        int versions = Versions;
        var x = await StartAStarArithmetic(start, end);
        if (versions != Versions)
        {
            return null;
        }

        return x;
    }

    public void Dispose()
    {
        Versions++;
        Maps.Clear();
        Barrierlist.Clear();
    }
}