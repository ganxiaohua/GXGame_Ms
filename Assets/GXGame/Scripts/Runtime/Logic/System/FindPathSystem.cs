using System.Collections.Generic;
using System.Diagnostics;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class FindPathSystem : UpdateReactiveSystem
    {
        private Dictionary<GridData, AStar> gridataAstarDic = new();

        public override void OnInitialize(World world)
        {
            base.OnInitialize(world);
        }

        protected override Collector GetTrigger(World world)
        {
            return Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, Components.PathFindingTargetPos);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.PathFindingTargetPos) && entity.HasComponent(Components.FindPathComponent);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var ecsEntity in entities)
            {
                Find(ecsEntity);
            }
        }

        private void Find(ECSEntity ecsEntity)
        {
            var worldPos = ecsEntity.GetWorldPos().Value;
            var targetPos = ecsEntity.GetPathFindingTargetPos().Value;
            var findPathComponent = ecsEntity.GetFindPathComponent().Value;
            var gridData = ecsEntity.GetGridDataComponent().Value;
            var start = gridData.WorldToCell(worldPos);
            if (!gridataAstarDic.TryGetValue(gridData, out var aStar))
            {
                aStar = new AStar();
                aStar.InitFindPath(gridData.GirdArea.x, gridData.GirdArea.y, gridData.ObstacleCells);
                gridataAstarDic[gridData] = aStar;
            }

            findPathComponent.IsFindPath = true;
            findPathComponent.Versions++;
            var path = findPathComponent.Path;
            path ??= new();
            bool b = aStar.Find(start, targetPos, path);
            if (!b)
            {
                return;
            }

            if (path[^1] == start)
            {
                path.RemoveAt(path.Count - 1);
            }

            path.Reverse();
            findPathComponent.IsFindPath = false;
            findPathComponent.Path = path;
            findPathComponent.NextIndex = 0;
            EditorPath(gridData, path);
            ecsEntity.SetFindPathComponent(findPathComponent);
        }

        [Conditional("UNITY_EDITOR")]
        private void EditorPath(GridData gridData, List<Vector2Int> path)
        {
#if UNITY_EDITOR
            gridData.FindPath = path;
#endif
        }

        public override void Dispose()
        {
            gridataAstarDic = null;
        }
    }
}