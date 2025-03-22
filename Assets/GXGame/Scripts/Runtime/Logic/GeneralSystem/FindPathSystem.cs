using System.Collections.Generic;
using System.Diagnostics;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class FindPathSystem : UpdateReactiveSystem, ISystemCarryover
    {
        public object Carryover { get; set; }

        private GridData gridData => (GridData) Carryover;

        private AStar aStarManager;

        private List<Vector2Int> path;

        public override void OnInitialize(World world)
        {
            base.OnInitialize(world);
            aStarManager = new AStar();
            aStarManager.InitFindPath(gridData.GirdArea.x, gridData.GirdArea.y, gridData.ObstacleCells);
            path = new List<Vector2Int>();
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
            var start = gridData.WorldToCell(worldPos);
            var target = gridData.WorldToCell(targetPos);

            findPathComponent.IsFindPath = true;
            findPathComponent.Versions++;
            bool b = aStarManager.Find(start, target, path);
            if (!b)
            {
                return;
            }

            if (path[0] == start)
            {
            }

            findPathComponent.IsFindPath = false;
            findPathComponent.Path = path;
            findPathComponent.NextIndex = 0;
            EditorPath(path);
            ecsEntity.SetFindPathComponent(findPathComponent);
        }

        [Conditional("UNITY_EDITOR")]
        private void EditorPath(List<Vector2Int> path)
        {
#if UNITY_EDITOR
            gridData.FindPath = path;
#endif
        }

        public override void Dispose()
        {
            aStarManager = null;
        }
    }
}