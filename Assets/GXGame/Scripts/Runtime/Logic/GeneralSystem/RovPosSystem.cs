﻿using GameFrame;
using RVO;
using UnityEngine;

namespace GXGame
{
    public class RovPosSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.RovAgent, Components.MoveDirection, Components.MoveSpeed);
            group = world.GetGroup(matcher);
            this.world = world;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Simulator.Instance.setTimeStep(world.DeltaTime);
            foreach (var entity in group)
            {
                var dir = entity.GetMoveDirection().Value;
                var speed = entity.GetMoveSpeed().Value;
                var rovId = entity.GetRovAgent().Value;
                Simulator.Instance.setAgentPrefVelocity(rovId, new Vector2(dir.x, dir.z) * speed);

                float angle = UnityEngine.Random.Range(0, 1) * 2.0f * (float) Mathf.PI;
                float dist = UnityEngine.Random.Range(0, 1) * 0.0001f;

                Simulator.Instance.setAgentPrefVelocity(rovId, Simulator.Instance.getAgentPrefVelocity(rovId) +
                                                               dist * new Vector2((float) Mathf.Cos(angle), (float) Mathf.Sin(angle)));
                Vector2 pos = Simulator.Instance.getAgentPosition(rovId);
                var map = entity.GetGridDataComponent().Value;
                Vector3 lastPos = new Vector3(pos.x, map.Pos.y, pos.y);
                entity.SetWorldPos(lastPos);
                var box = entity.GetBoxCollider();
                if (box == null)
                    continue;
                box.Value.position = lastPos;
            }

            Simulator.Instance.doStep();
        }

        public void Dispose()
        {
        }
    }
}