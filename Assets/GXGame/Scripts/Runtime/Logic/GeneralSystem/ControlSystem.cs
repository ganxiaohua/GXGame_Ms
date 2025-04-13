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
        private Group group;
        private World world;
        private ECSEntity entity;
        private Group cameraGroup;
        private CapsuleColliderComponent capsuleColliderComponent;
        private GroundCollision groundMsg;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.GXInput);
            group = world.GetGroup(matcher);
            matcher = Matcher.SetAll(Components.CameraComponent);
            cameraGroup = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                this.entity = entity;
                var capsuleCollider = entity.GetCapsuleColliderComponent();
                collisionMsg = entity.GetCollisionMsgComponent().Value;
                this.capsuleColliderComponent = capsuleCollider;
                groundMsg = entity.GetGroundCollisionComponent().Value;
                InputMove();
                entity.SetCapsuleColliderComponent(capsuleCollider.Value);
            }
        }


        public void Dispose()
        {
            Clear();
        }
    }
}