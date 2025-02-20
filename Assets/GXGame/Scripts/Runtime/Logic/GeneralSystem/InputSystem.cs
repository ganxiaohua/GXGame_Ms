using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class InputSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Vector3 inputPos;
        private Group playerGroup;
        private Group cameraGroup;
        private Dictionary<KeyCode, int> keyCode;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.Player, Components.GXInput, Components.FaceDirection)
                .NoneOf(Components.SkillComponent);
            playerGroup = world.GetGroup(matcher);
            matcher = Matcher.SetAll(Components.CameraComponent);
            cameraGroup = world.GetGroup(matcher);
            keyCode = new();
            keyCode.Add(KeyCode.A, -1);
            keyCode.Add(KeyCode.D, 1);
            keyCode.Add(KeyCode.W, 1);
            keyCode.Add(KeyCode.S, -1);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            Move();
            Jump();
            CameraInput();
        }

        private void Move()
        {
            bool set = false;
            if (inputPos != Vector3.zero)
            {
                set = true;
                inputPos.Set(0, 0, 0);
            }

            int index = 0;
            foreach (var variable in keyCode)
            {
                if (Input.GetKey(variable.Key))
                {
                    set = true;
                    if (index < 2)
                    {
                        inputPos.x = variable.Value;
                    }
                    else
                    {
                        inputPos.z = variable.Value;
                    }
                }

                index++;
            }

            if (!set)
                return;
            foreach (var entity in playerGroup)
            {
                entity.SetMoveDirection(inputPos);
            }
        }

        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (var entity in playerGroup)
                {
                    entity.SetYAxisAcceleration(true);
                }
            }
        }

        private void CameraInput()
        {
            float yaw = 0;
            float pitch = 0;
            if (Input.GetMouseButton(1))
            {
                yaw = Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
                pitch = -1 * Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
            }
            foreach (var entity in cameraGroup)
            {
                entity.SetMoveDirection(new Vector3(yaw, pitch, 0));
            }
        }

        public void Dispose()
        {
        }
    }
}