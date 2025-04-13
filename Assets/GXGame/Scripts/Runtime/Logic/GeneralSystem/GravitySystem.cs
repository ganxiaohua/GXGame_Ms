using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class GravitySystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private World world;
        private Group group;
        private GroundCollision groundMsg;
        private CollisionMsg collisionMsg;

        public void OnInitialize(World world)
        {
            this.world = world;
            Matcher matcher = Matcher.SetAll(Components.GravityComponent, Components.GroundCollisionComponent, Components.CollisionMsgComponent);
            group = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                groundMsg = entity.GetGroundCollisionComponent().Value;
                collisionMsg = entity.GetCollisionMsgComponent().Value;
                GravityJump(entity);
            }
        }


        private void GravityJump(ECSEntity entity)
        {
            var gravity = entity.GetGravityComponent().Value;
            var jumpSpeed = entity.GetYAxisASpeed().Value;
            bool fg = IsFallingOrglide();
            var velocity = entity.GetGravityDirComponent().Value;
            if (fg)
            {
                velocity.y += -gravity * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }

            bool canJump = (groundMsg.OnGround) && groundMsg.Angle <= collisionMsg.maxJumpAngle && !fg;
            if (canJump && jumpSpeed != 0)
            {
                velocity = Vector3.Lerp(Vector3.up, (groundMsg.RaycastHit.normal).normalized, collisionMsg.jumpAngleWeightFactor).normalized * jumpSpeed;
            }

            velocity.y = Mathf.Min(velocity.y, gravity * 2);
            entity.SetYAxisASpeed(0);
            entity.SetGravityDirComponent(velocity);
        }


        private bool IsFallingOrglide()
        {
            return !(groundMsg.OnGround && groundMsg.Angle <= collisionMsg.maxWalkingAngle);
        }

        public void Dispose()
        {
        }
    }
}