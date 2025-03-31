using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputPcSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Dictionary<KeyCode, Vector2> keyCode;
        private Group group;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.OperationComponent);
            group = world.GetGroup(matcher);
            keyCode = new();
            keyCode.Add(KeyCode.A, new Vector2(-1, 0));
            keyCode.Add(KeyCode.D, new Vector2(1, 0));
            keyCode.Add(KeyCode.W, new Vector2(0, 1));
            keyCode.Add(KeyCode.S, new Vector2(0, -1));
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                var operationComp = entity.GetOperationComponent();
                Move(operationComp);
                Jump(operationComp);
                Interaction(operationComp);
                CameraInput(operationComp);
                entity.SetOperationComponent(operationComp.Value);
            }
        }

        private void Move(OperationComponent comp)
        {
            comp.Value.MoveDir = Vector2.zero;
            foreach (var variable in keyCode)
            {
                if (Input.GetKey(variable.Key))
                {
                    comp.Value.MoveDir += variable.Value;
                }
            }
        }

        private void Jump(OperationComponent comp)
        {
            comp.Value.Jump = Input.GetKey(KeyCode.Space);
        }

        private void CameraInput(OperationComponent comp)
        {
            float yaw = 0;
            float pitch = 0;

            if (Input.GetMouseButton(1))
            {
                yaw = Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
                pitch = -1 * Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
            }

            comp.Value.CameraDir.x = yaw;
            comp.Value.CameraDir.y = pitch;
        }

        private void Interaction(OperationComponent comp)
        {
            comp.Value.Interaction = Input.GetKey(KeyCode.F);
        }

        public void Dispose()
        {
        }
    }
}