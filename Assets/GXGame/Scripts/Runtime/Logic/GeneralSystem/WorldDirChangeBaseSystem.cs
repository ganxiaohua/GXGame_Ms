﻿using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldDirChangeBaseSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate, Components.FaceDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return !entity.HasComponent(Components.GXInput) && entity.HasComponent((Components.WorldRotate)) &&
                   entity.HasComponent(Components.FaceDirection) &&
                   entity.HasComponent(Components.DirectionSpeed);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            Common(entities);
        }

        private void Common(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetFaceDirection().Value;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().Value;
                    Vector3 nowDir = entity.GetWorldRotate().Value * Vector3.forward;
                    float angle = speed * World.DeltaTime;
                    Vector3 curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
                    var roa = Quaternion.LookRotation(curDir);
                    entity.SetWorldRotate(roa);
                    var box = entity.GetBoxColliderComponent();
                    if (box == null)
                        continue;
                    box.Value.Go.rotation = roa;
                }
            }
        }


        public override void Dispose()
        {
        }
    }
}