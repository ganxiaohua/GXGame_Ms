using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 检测地面碰撞信息
    /// </summary>
    public class GroundSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private Group group;
        private World world;
        private RaycastHit[] ray;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAny(Components.BoxColliderComponent, Components.CapsuleColliderComponent).Any(Components.GroundCollisionComponent);
            this.world = world;
            group = world.GetGroup(matcher);
            ray = new RaycastHit[10];
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                if (entity.HasComponent(Components.CapsuleColliderComponent))
                    CheckGroundedCapsule(entity);
                else if (entity.HasComponent(Components.BoxColliderComponent))
                    CheckBoxCapsule(entity);
            }
        }

        private void CheckGroundedCapsule(ECSEntity entity)
        {
            var pos = entity.GetWorldPos().Value;
            var rot = entity.GetWorldRotate().Value;
            var capsuleColliderData = entity.GetCapsuleColliderComponent().Value;
            var collisionMsg = entity.GetCollisionMsgComponent().Value;
            var groundCollision = entity.GetGroundCollisionComponent().Value;
            bool onGround = CollisionDetection.CapsuleCastNonAlloc(capsuleColliderData.Go.transform,
                ray, capsuleColliderData.CapsuleCollider, pos, rot,
                Vector3.down, collisionMsg.groundDist, out RaycastHit groundHit,
                collisionMsg.MaskLayer, collisionMsg.skinWidth);
            float angle = Vector3.Angle(groundHit.normal, Vector3.up);
            groundCollision.OnGround = onGround;
            groundCollision.Angle = angle;
            groundCollision.RaycastHit = groundHit;
            entity.SetGroundCollisionComponent(groundCollision);
        }

        private void CheckBoxCapsule(ECSEntity entity)
        {
        }


        public void Dispose()
        {
            // TODO 在此释放托管资源
        }
    }
}