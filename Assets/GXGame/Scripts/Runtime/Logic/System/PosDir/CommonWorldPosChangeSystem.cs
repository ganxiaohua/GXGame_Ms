using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class CommonWorldPosChangeSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, Components.MoveDirection, Components.ForceWorldPos);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.MoveSpeed) &&
                   !entity.HasComponent(Components.RovAgent) &&
                   !entity.HasComponent(Components.CollisionMsgComponent);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var forceworldPosComp = entity.GetForceWorldPos();
                if (forceworldPosComp == null)
                {
                    var dir = entity.GetMoveDirection().Value;
                    var distance = entity.GetMoveSpeed().Value * World.DeltaTime;
                    var pos = entity.GetWorldPos().Value;
                    pos += (dir.normalized * distance);
                    entity.SetWorldPos(pos);
                    var box = entity.GetBoxColliderComponent();
                    if (box == null)
                        continue;
                    box.Value.Go.position = pos;
                }
                else
                {
                    entity.SetWorldPos(forceworldPosComp.Value);
                    var box = entity.GetBoxColliderComponent();
                    if (box == null)
                        continue;
                    box.Value.Go.position = forceworldPosComp.Value;
                }
            }
        }

        public override void Dispose()
        {
        }
    }
}