﻿using GameFrame;

namespace GXGame
{
    public class WorldPosChangeSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.MoveSpeed).NoneOf(Components.GXInput, Components.RovAgent);
            group = world.GetGroup(matcher);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Common();
        }

        private void Common()
        {
            foreach (var entity in group)
            {
                var dir = entity.GetMoveDirection().Value;
                var distance = entity.GetMoveSpeed().Value * world.DeltaTime;
                var pos = entity.GetWorldPos().Value;
                pos += (dir.normalized * distance);
                entity.SetWorldPos(pos);
                var box = entity.GetBoxColliderComponent();
                if (box == null)
                    continue;
                box.Value.Go.position = pos;
            }
        }

        public void Dispose()
        {
        }
    }
}