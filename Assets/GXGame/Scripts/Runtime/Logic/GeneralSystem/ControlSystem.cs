using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 碰撞系统
    /// </summary>
    public partial class ControlSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private RaycastHit[] raycastHit = new RaycastHit[5];
        private List<RaycastHit> collisionWithObjectLayer;
        private Group group;
        private World world;
        private ECSEntity entity;
        private Group cameraGroup;
        private CapsuleCollider capsuleCollider;
        private UnityEngine.CapsuleCollider unityCapsuleCollider;
        private (bool onGround, float groundAngle, RaycastHit hit) groundMsg;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.FaceDirection, Components.CapsuleCollider);
            group = world.GetGroup(matcher);
            matcher = Matcher.SetAll(Components.CameraComponent);
            cameraGroup = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                this.entity = entity;
                var capsuleCollider = entity.GetCapsuleCollider();
                collisionMsg = entity.GetCollisionMsgComponent().Value;
                this.capsuleCollider = capsuleCollider;
                unityCapsuleCollider = capsuleCollider.Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
                groundMsg = CheckGrounded();
                SetInput();
                entity.SetCapsuleCollider(capsuleCollider.Value);
            }
        }


        // private void ObjectCollision(ECSEntity entity)
        // {
        //     if (collisionWithObjectLayer.Count != 0)
        //     {
        //         var hit = entity.GetRaycastHitMsg();
        //         if (hit == null)
        //         {
        //             entity.AddRaycastHitMsg(new List<RaycastHit>());
        //             hit = entity.GetRaycastHitMsg();
        //         }
        //
        //         foreach (var t in collisionWithObjectLayer)
        //         {
        //             hit.Value.Add(t);
        //         }
        //
        //         entity.SetRaycastHitMsg(hit.Value);
        //     }
        // }


        public void Dispose()
        {
            Clear();
            collisionWithObjectLayer.Clear();
        }
    }
}