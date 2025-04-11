using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class OprationSystem : UpdateReactiveSystem
    {
        private Group inputs;
        private Group cameras;
        private World world;

        public override void OnInitialize(World world)
        {
            base.OnInitialize(world);
            Matcher matcher = Matcher.SetAll(Components.GXInput);
            inputs = world.GetGroup(matcher);
            matcher = Matcher.SetAll(Components.CameraComponent);
            cameras = world.GetGroup(matcher);
        }


        protected override Collector GetTrigger(World world)
        {
            return Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, Components.OperationComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var operationComponent in entities)
            {
                UpdateCamera(operationComponent);
                UpdateInput(operationComponent);
                UpdateFeedBack(operationComponent);
            }
        }

        private void UpdateCamera(ECSEntity ecsEntity)
        {
            var v = ecsEntity.GetOperationComponent().Value;
            foreach (var camera in cameras)
            {
                camera.SetMoveDirection(v.CameraDir);
            }
        }

        private void UpdateInput(ECSEntity ecsEntity)
        {
            var v = ecsEntity.GetOperationComponent().Value;
            foreach (var input in inputs)
            {
                input.SetMoveDirection(new Vector3(v.MoveDir.x, 0, v.MoveDir.y));
                input.SetYAxisASpeed(v.Jump ? 5 : 0);
            }
        }


        public override void Dispose()
        {
        }
    }
}