using System.Collections.Generic;
using UnityEngine;

namespace GXGame.Editor
{
    public partial class GridManager
    {
        private Material buildMaterial;
        private List<Mesh> buildAreaMeshList = new List<Mesh>();


        private void BuildObstacle(GridData gridridData)
        {
            if (gridridData.ObstacleCell == null)
                return;
            int index = 0;
            foreach (var item in gridridData.ObstacleCell)
            {
                if (buildAreaMeshList.Count <= index)
                {
                    buildAreaMeshList.Add(null);
                }

                Mesh mesh = buildAreaMeshList[index];
                DrawGrid.DrawQuadGizmos(GridData, new RectInt(item.x, item.y, 1, 1), ref mesh, ref buildMaterial, Color.gray);
                buildAreaMeshList[index] = mesh;
                index++;
            }
        }


        private void ClearAreaMesh()
        {
            foreach (var item in buildAreaMeshList)
            {
                DestroyImmediate(item);
            }

            buildMaterial = null;
        }
    }
}