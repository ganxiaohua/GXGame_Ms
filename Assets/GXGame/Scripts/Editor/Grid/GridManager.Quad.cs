using System.Collections.Generic;
using UnityEngine;

namespace GXGame.Editor
{
    public partial class GridManager
    {
        private Material buildMaterial;
        private Material buildMaterial2;
        private List<Mesh> barrierMeshList = new List<Mesh>();
        private List<Mesh> findPathMeshList = new List<Mesh>();
        private void BuildPromptMesh(GridData gridridData)
        {
            BuildObstacle(gridridData);
            BuildFindPath(gridridData);
        }

        private void BuildObstacle(GridData gridridData)
        {
            if (gridridData.ObstacleCell == null)
                return;
            int index = 0;
            foreach (var item in gridridData.ObstacleCell)
            {
                if (barrierMeshList.Count <= index)
                {
                    barrierMeshList.Add(null);
                }

                Mesh mesh = barrierMeshList[index];
                DrawGrid.DrawQuadGizmos(GridData, new RectInt(item.x, item.y, 1, 1), ref mesh, ref buildMaterial, Color.gray);
                barrierMeshList[index] = mesh;
                index++;
            }
        }
        
        private void BuildFindPath(GridData gridridData)
        {
            if (gridridData.FindPath == null)
                return;
            int index = 0;
            foreach (var item in gridridData.FindPath)
            {
                if (findPathMeshList.Count <= index)
                {
                    findPathMeshList.Add(null);
                }

                Mesh mesh = findPathMeshList[index];
                DrawGrid.DrawQuadGizmos(GridData, new RectInt(item.x, item.y, 1, 1), ref mesh, ref buildMaterial2, gridridData.FindPathColor);
                findPathMeshList[index] = mesh;
                index++;
            }
        }
        
        private void ClearAreaMesh()
        {
            foreach (var item in barrierMeshList)
            {
                DestroyImmediate(item);
            }
            
            foreach (var item in findPathMeshList)
            {
                DestroyImmediate(item);
            }

            buildMaterial = null;
        }
    }
}