using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GXGame
{
    public partial class FindPathSystem : UpdateReactiveSystem, ISystemCarryover
    {
        public object Carryover { get; set; }

        private GridData gridData => (GridData) Carryover;

        private Astar aStarManager;

        public override void OnInitialize(World world)
        {
            base.OnInitialize(world);
            aStarManager = new Astar();
            aStarManager.InitMap(gridData);
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
                Find(ecsEntity).Forget();
            }
        }

        private async UniTaskVoid Find(ECSEntity ecsEntity)
        {
            var worldPos = ecsEntity.GetWorldPos().Value;
            var targetPos = ecsEntity.GetPathFindingTargetPos().Value;
            var findPathComponent = ecsEntity.GetFindPathComponent().Value;
            var start = gridData.WorldToCell(worldPos);
            var target = gridData.WorldToCell(targetPos);
            findPathComponent.IsFindPath = true;
            findPathComponent.Versions++;
            var ver = findPathComponent.Versions;
            var pathData = await aStarManager.Find(new Vector2Int(start.x, start.z), new Vector2Int(target.x, target.z));
            findPathComponent.IsFindPath = false;
            if (pathData == null || ver!= findPathComponent.Versions)
                return;
            findPathComponent.Path = pathData;
            findPathComponent.NextIndex = 0;
            EditorPath(pathData);
            ecsEntity.SetFindPathComponent(findPathComponent);
        }

        [Conditional("UNITY_EDITOR")]
        private void EditorPath(List<Vector2Int> path)
        {
            gridData.FindPath = path;
        }

        public override void Dispose()
        {
            aStarManager.Dispose();
        }
    }
}