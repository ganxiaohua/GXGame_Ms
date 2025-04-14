using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class BeingCaughtSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.BeingCaughtComponent);
            group = world.GetGroup(matcher);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                var beingCaught = entity.GetBeingCaughtComponent().Value;
                if (beingCaught.Holder != null && beingCaught.Holder.IsAction)
                {
                    var holderRot = beingCaught.Holder.GetWorldRotate().Value;
                    var dir = (holderRot * Vector3.forward).normalized;
                    var wordpos = beingCaught.Holder.GetWorldPos().Value;
                    var pos = wordpos + dir * 0.5f + beingCaught.Offset;
                    if (entity.HasComponent(Components.BoxColliderComponent))
                        entity.RemoveComponent(Components.BoxColliderComponent);
                    entity.AddOrSetForceWorldPos(pos);
                    entity.AddOrSetForceWorldRotate(holderRot);
                }
            }
        }


        public void Dispose()
        {
        }
    }
}