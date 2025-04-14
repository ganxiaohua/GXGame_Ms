using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 碰撞系统
    /// </summary>
    public partial class CollisionWorldPosDirSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private RaycastHit[] raycastHit = new RaycastHit[5];
        private Group group;
        private World world;
        private ECSEntity entity;
        private Group cameraGroup;
        private GroundCollision groundMsg;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.CollisionMsgComponent);
            group = world.GetGroup(matcher);
            matcher = Matcher.SetAll(Components.CameraComponent);
            cameraGroup = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                this.entity = entity;
                collisionMsg = entity.GetCollisionMsgComponent().Value;
                groundMsg = entity.GetGroundCollisionComponent().Value;
                CollisionMovement();
            }
        }


        public void Dispose()
        {
            Clear();
        }
    }
}