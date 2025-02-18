using System.Collections.Generic;
using System.Linq;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class ControlSystem
    {
        private float groundDist = 0.01f;
        private float epsilon = 0.001f;
        private float skinWidth = 0.01f;
        private float anglePower = 2.0f;
        private float maxWalkingAngle = 60f;
        private float maxJumpAngle = 80f;
        private float jumpAngleWeightFactor = 0.1f;
        private float stepUpDepth = 5f; 
        public const float MaxAngleShoveDegrees = 180.0f - BufferAngleShove;
        public const float BufferAngleShove = 120.0f;
        private Vector3 velocity;
        protected static Collider[] OverlapCache = new Collider[20];


        private void SetInput()
        {
            var dir = entity.GetMoveDirection().Value;
            var pos = entity.GetWorldPos().Value;
            var rot = entity.GetWorldRotate().Value;
            FollowGround(ref pos, ref rot);
            var initPos = pos;
            pos += PushOutOverlapping(pos, rot, 100 * Time.deltaTime, skinWidth / 2);
            var moveSpeed = entity.GetMoveSpeed().Value;
            bool fg = IsFallingOrglide();
            dir = !fg ? Vector3.ProjectOnPlane(dir, groundMsg.hit.normal) : dir;
            dir = dir.normalized * (moveSpeed * Time.deltaTime);
            GravityJump();
            pos = MovePlayer(pos, dir);
            pos = MovePlayer(pos, velocity * Time.deltaTime);
            // pos = SnapPlayerDown(pos);
            rot = CalculateWorldRotate(rot);
            capsuleCollider.Value.position = pos;
            capsuleCollider.Value.rotation = rot;
            entity.SetWorldPos(pos);
            entity.SetWorldRotate(rot);
            UpdateMovingGround(initPos, rot, pos - initPos);
        }

        private Quaternion CalculateWorldRotate(Quaternion rot)
        {
            var dir = entity.GetFaceDirection().Value;
            float speed = entity.GetDirectionSpeed().Value;
            if (!groundMsg.onGround)
                speed /= 2;
            Vector3 nowDir = rot * Vector3.forward;
            float angle = speed * Time.deltaTime * world.Multiple;
            var curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            var drot = Quaternion.LookRotation(curDir);
            return drot;
        }

        private void GravityJump()
        {
            var gravity = entity.GetGravity().Value;
            var jumpSpeed = entity.GetYAxisASpeed().Value;
            var yAxis = entity.GetYAxisAcceleration().Value;
            bool fg = IsFallingOrglide();
            if (fg)
            {
                velocity.y += -gravity * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }

            bool canJump = (groundMsg.onGround) && groundMsg.groundAngle <= maxJumpAngle && !fg;
            if (canJump && yAxis)
            {
                velocity = Vector3.Lerp(Vector3.up, (groundMsg.hit.normal).normalized, jumpAngleWeightFactor).normalized * jumpSpeed;
            }

            entity.SetYAxisAcceleration(false);
        }


        private  Vector3 PushOutOverlapping(Vector3 position, Quaternion rotation, float maxDistance, float skinWidth = 0.0f)
        {
            Vector3 pushed = Vector3.zero;
            var count = GetOverlapping(position, rotation, ~0, QueryTriggerInteraction.Collide, skinWidth);
            for (int i = 0; i < count; i++)
            {
                var overlap = OverlapCache[i];
                Physics.ComputePenetration(
                    unityCapsuleCollider, position, rotation,
                    overlap, overlap.gameObject.transform.position, overlap.gameObject.transform.rotation,
                    out Vector3 direction, out float distance
                );
                Vector3 push = direction.normalized * (distance);
                pushed += push;
                position += push;
            }

            return Vector3.ClampMagnitude(pushed, maxDistance);
        }


        private (bool, float, RaycastHit) CheckGrounded()
        {
            var pos = entity.GetWorldPos().Value;
            var rot = entity.GetWorldRotate().Value;
            bool onGround = CastSelf(pos, rot, Vector3.down, groundDist, out RaycastHit groundHit, skinWidth);
            float angle = Vector3.Angle(groundHit.normal, Vector3.up);
            return (onGround, angle, groundHit);
        }

        private bool IsFallingOrglide()
        {
            return !(groundMsg.onGround && groundMsg.groundAngle <= maxWalkingAngle);
        }

        private Vector3 MovePlayer(Vector3 position, Vector3 movement)
        {
            var rotation = entity.GetWorldRotate().Value;
            Vector3 remaining = movement;
            int bounces = 0;
            //弹跳次数小于最大弹跳次数  &&  移动位置比最小可移动位置大.
            while (bounces < 5 && remaining.magnitude > epsilon)
            {
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit, skinWidth))
                {
                    position += remaining;
                    break;
                }

                if (Vector3.Dot(remaining, movement) < 0)
                {
                    break;
                }

                float fraction = hit.distance / distance;
                Vector3 deltaBounce = remaining * fraction;
                deltaBounce = deltaBounce.normalized * Mathf.Max(0, deltaBounce.magnitude - epsilon);
                //这里是计算刚好碰到撞击点的距离
                position += deltaBounce;
                //撞击点朝着移动方向延伸出去的一段距离.
                remaining *= (1 - Mathf.Max(0, deltaBounce.magnitude / distance));
                Vector3 planeNormal = hit.normal;
                //上楼梯计算
                bool perpendicularBounce = CheckPerpendicularBounce(hit, remaining);
                Vector3 snappedMomentum = remaining;
                Vector3 snappedPosition = position;
                if (perpendicularBounce && AttemptSnapUp(hit, ref snappedMomentum, ref snappedPosition, rotation))
                {
                    position = snappedPosition;
                    continue;
                }
                
                //计算出碰撞曲面与操作方向的夹角
                float angleBetween = Vector3.Angle(hit.normal, remaining);
                //操作方向的夹角如果大于KCCUtils.MaxAngleShoveDegrees,则使用KCCUtils.MaxAngleShoveDegrees
                float normalizedAngle = Mathf.Max(angleBetween - BufferAngleShove, 0) / MaxAngleShoveDegrees;
                remaining *= Mathf.Pow(Mathf.Abs(1 - normalizedAngle), anglePower);
                Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

                if (projected.magnitude + epsilon < remaining.magnitude)
                {
                    remaining = Vector3.ProjectOnPlane(remaining, Vector3.up).normalized * remaining.magnitude;
                }
                else
                {
                    remaining = projected;
                }
                bounces++;
            }
            return position;
        }

        /// <summary>
        /// 下楼梯的时候速度要加快
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 SnapPlayerDown(Vector3 position)
        {
            var rotation = entity.GetWorldRotate().Value;
            bool closeToGround = CastSelf(
                position,
                rotation,
                Vector3.down,
                skinWidth,
                out RaycastHit groundHit);
            if (closeToGround && groundHit.distance > 0)
            {
                var offset = Vector3.ClampMagnitude( Vector3.down * groundHit.distance, 3 * Time.deltaTime);
                position += offset;
            }

            return position;
        }

        private void Clear()
        {
            velocity = Vector3.zero;
        }
    }
}