using System.Collections.Generic;
using System.Linq;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class CollisionSystem
    {
        private CapsuleCollider capsuleCollider;
        private ECSEntity entity;
        private UnityEngine.CapsuleCollider unityCapsuleCollider;
        private float groundDist = 0.01f;
        private float epsilon = 0.001f;
        private float anglePower = 0.5f;
        private float maxWalkingAngle = 60f;
        private float maxJumpAngle = 80f;
        private float jumpAngleWeightFactor = 0.1f;
        public const float MaxAngleShoveDegrees = 180.0f - BufferAngleShove;
        public const float BufferAngleShove = 120.0f;
        private Vector3 velocity;

        private void SetWolrdPos2(ECSEntity entity, CapsuleCollider capsuleCollider)
        {
            this.entity = entity;
            this.capsuleCollider = capsuleCollider;
            unityCapsuleCollider = capsuleCollider.Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            var dir = entity.GetMoveDirection().Value;
            var pos = entity.GetWorldPos().Value;
            var moveSpeed = entity.GetMoveSpeed().Value;
            dir = dir.normalized * moveSpeed * Time.deltaTime;
            GravityJump(ref dir);
            pos = MovePlayer(dir + velocity * Time.deltaTime);
            capsuleCollider.Value.position = pos;
            entity.SetWorldPos(pos);
        }

        private void GravityJump(ref Vector3 movement)
        {
            var gravity = entity.GetGravity().Value;
            var jumpSpeed = entity.GetYAxisASpeed().Value;
            var yAxis = entity.GetYAxisAcceleration().Value;
            (bool onGround, float groundAngle) = CheckGrounded(out RaycastHit groundHit);
            bool falling = !(onGround && groundAngle <= maxWalkingAngle);
            if (falling)
            {
                velocity.y += -gravity * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }

            bool canJump = (onGround) && groundAngle <= maxJumpAngle && !falling;
            if (canJump && yAxis)
            {
                velocity = (Vector3.Lerp(Vector3.up, (groundHit.normal).normalized, jumpAngleWeightFactor) + movement.normalized).normalized * jumpSpeed;
            }

            movement = !falling ? Vector3.ProjectOnPlane(movement, groundHit.normal) : Vector3.zero;
            entity.SetYAxisAcceleration(false);
        }

        public bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit)
        {
            Vector3 center = rot * unityCapsuleCollider.center + pos;
            float radius = unityCapsuleCollider.radius;
            float height = unityCapsuleCollider.height;

            Vector3 bottom = center + rot * Vector3.down * (height / 2 - radius);
            Vector3 top = center + rot * Vector3.up * (height / 2 - radius);

            int count = Physics.CapsuleCastNonAlloc(top, bottom, radius, dir, raycastHit, dist, ~0, QueryTriggerInteraction.Ignore);
            float directDist = float.MaxValue;
            bool didHit = false;
            hit = default;
            for (int i = 0; i < count; i++)
            {
                var tempHit = raycastHit[i];
                //过滤自己 选出距离最近的
                if (tempHit.transform != capsuleCollider.Value.transform && directDist > tempHit.distance)
                {
                    hit = tempHit;
                    directDist = tempHit.distance;
                    didHit = true;
                }
            }

            return didHit;
        }


        private (bool, float) CheckGrounded(out RaycastHit groundHit)
        {
            var dir = entity.GetMoveDirection().Value;
            var pos = entity.GetWorldPos().Value;
            var rot = entity.GetWorldRotate().Value;
            bool onGround = CastSelf(pos, rot, Vector3.down, groundDist, out groundHit);
            float angle = Vector3.Angle(groundHit.normal, Vector3.up);
            return (onGround, angle);
        }

        public Vector3 MovePlayer(Vector3 movement)
        {
            var position = entity.GetWorldPos().Value;
            var rotation = entity.GetWorldRotate().Value;

            Vector3 remaining = movement;

            int bounces = 0;
            //弹跳次数小于最大弹跳次数  &&  移动位置比最小可移动位置大.
            while (bounces < 5 && remaining.magnitude > epsilon)
            {
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit))
                {
                    position += remaining;

                    break;
                }

                if (hit.distance == 0)
                {
                    break;
                }

                float fraction = hit.distance / distance;
                //这里是计算刚好碰到撞击点的距离
                position += remaining * (fraction);
                //向外轻轻推出一个皮肤的宽度. 防止卡在墙里
                position += hit.normal * epsilon * 2;
                //撞击点朝着移动方向延伸出去的一段距离.
                remaining *= (1 - fraction);
                Vector3 planeNormal = hit.normal;
                //计算出碰撞曲面与操作方向的夹角
                float angleBetween = Vector3.Angle(hit.normal, remaining) - 90.0f;
                //操作方向的夹角如果大于KCCUtils.MaxAngleShoveDegrees,则使用KCCUtils.MaxAngleShoveDegrees
                angleBetween = Mathf.Min(MaxAngleShoveDegrees, Mathf.Abs(angleBetween));

                //有几分之几个KCCUtils.MaxAngleShoveDegrees;
                float normalizedAngle = angleBetween / MaxAngleShoveDegrees;

                remaining *= Mathf.Pow(1 - normalizedAngle, anglePower) * 0.9f + 0.1f;

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
    }
}