﻿using UnityEngine;

namespace GXGame
{
    public partial class CollisionWorldPosDirSystem
    {
        public const float MaxAngleShoveDegrees = 180.0f - BufferAngleShove;
        public const float BufferAngleShove = 120.0f;
        private CollisionMsg collisionMsg;
        protected Collider[] OverlapCache = new Collider[20];

        private void CollisionMovement()
        {
            var dir = entity.GetMoveDirection().Value;
            var pos = entity.GetWorldPos().Value;
            var rot = entity.GetWorldRotate().Value;
            var gravityDir = entity.GetGravityDirComponent().Value;
            foreach (var camera in cameraGroup)
            {
                var camerRotValue = camera.GetWorldRotate().Value;
                dir = camerRotValue * dir;
                dir.y = 0;
                entity.SetFaceDirection(dir);
            }

            FollowGround(ref pos, ref rot);
            pos += PushOutOverlapping(pos, rot, 100 * Time.deltaTime, collisionMsg.skinWidth / 2);
            var moveSpeed = entity.GetMoveSpeed().Value;
            dir = (groundMsg.OnGround && groundMsg.Angle <= collisionMsg.maxWalkingAngle) ? Vector3.ProjectOnPlane(dir, groundMsg.RaycastHit.normal) : dir;
            dir = dir.normalized * (moveSpeed * Time.deltaTime);
            pos = MovePlayer(pos, dir);
            pos = MovePlayer(pos, gravityDir * Time.deltaTime);
            if (groundMsg.OnGround && gravityDir.y <= 0)
                pos = SnapPlayerDown(pos);
            rot = CalculateWorldRotate(rot);
            entity.SetWorldPos(pos);
            entity.SetWorldRotate(rot);
            UpdateMovingGround(pos, rot);
            var capsuleColliderComponent = entity.GetCapsuleColliderComponent();
            var boxColliderComponent = entity.GetBoxColliderComponent();
            if (capsuleColliderComponent != null)
            {
                capsuleColliderComponent.Value.Go.position = pos;
                capsuleColliderComponent.Value.Go.rotation = rot;
            }
            else if (boxColliderComponent != null)
            {
                boxColliderComponent.Value.Go.position = pos;
                boxColliderComponent.Value.Go.rotation = rot;
            }
        }

        private Quaternion CalculateWorldRotate(Quaternion rot)
        {
            var dir = entity.GetFaceDirection().Value;
            float speed = entity.GetDirectionSpeed().Value;
            if (!groundMsg.OnGround)
                speed /= 2;
            Vector3 nowDir = rot * Vector3.forward;
            float angle = speed * Time.deltaTime * world.Multiple;
            var curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            var drot = Quaternion.LookRotation(curDir);
            return drot;
        }


        private Vector3 PushOutOverlapping(Vector3 position, Quaternion rotation, float maxDistance, float skinWidth = 0.0f)
        {
            Vector3 pushed = Vector3.zero;
            Collider collider = null;
            var capsuleColliderComponent = entity.GetCapsuleColliderComponent();
            var boxColliderComponent = entity.GetBoxColliderComponent();
            int count = 0;
            if (capsuleColliderComponent != null)
            {
                collider = capsuleColliderComponent.Value.CapsuleCollider;
                count = CollisionDetection.OverlapCapsuleNonAlloc(capsuleColliderComponent.Value.Go.transform, OverlapCache,
                    capsuleColliderComponent.Value.CapsuleCollider, position,
                    rotation,
                    collisionMsg.MaskLayer, QueryTriggerInteraction.Collide, skinWidth);
            }
            else if (boxColliderComponent != null)
            {
                collider = boxColliderComponent.Value.BoxCollider;
                count = CollisionDetection.OverlapBoxNonAlloc(boxColliderComponent.Value.Go.transform, OverlapCache, position, rotation,
                    boxColliderComponent.Value.BoxCollider.size, collisionMsg.MaskLayer, QueryTriggerInteraction.Collide);
            }

            for (int i = 0; i < count; i++)
            {
                var overlap = OverlapCache[i];
                Physics.ComputePenetration(
                    collider, position, rotation,
                    overlap, overlap.gameObject.transform.position, overlap.gameObject.transform.rotation,
                    out Vector3 direction, out float distance
                );
                Vector3 push = direction.normalized * (distance);
                pushed += push;
                position += push;
            }

            return Vector3.ClampMagnitude(pushed, maxDistance);
        }


        private Vector3 MovePlayer(Vector3 position, Vector3 movement)
        {
            var rotation = entity.GetWorldRotate().Value;
            Vector3 remaining = movement;
            int bounces = 0;
            //弹跳次数小于最大弹跳次数  &&  移动位置比最小可移动位置大.
            while (bounces < 5 && remaining.magnitude >= collisionMsg.epsilon)
            {
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit, collisionMsg.MaskLayer, collisionMsg.skinWidth))
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
                deltaBounce = deltaBounce.normalized * Mathf.Max(0, deltaBounce.magnitude - collisionMsg.epsilon);
                //这里是计算刚好碰到撞击点的距离
                position += deltaBounce;
                //撞击点朝着移动方向延伸出去的一段距离.
                remaining *= (1 - Mathf.Max(0, deltaBounce.magnitude / distance));
                Vector3 planeNormal = hit.normal;
                //上楼梯计算
                bool perpendicularBounce = CheckPerpendicularBounce(hit, remaining);
                Vector3 snappedMomentum = remaining;
                Vector3 snappedPosition = position;
                if (groundMsg.OnGround && perpendicularBounce && AttemptSnapUp(hit, ref snappedMomentum, ref snappedPosition, rotation))
                {
                    if (snappedPosition.magnitude >= collisionMsg.epsilon)
                    {
                        position = snappedPosition;
                    }

                    continue;
                }

                //计算出碰撞曲面与操作方向的夹角
                float angleBetween = Vector3.Angle(hit.normal, remaining);
                float normalizedAngle = Mathf.Max(angleBetween - BufferAngleShove, 0) / MaxAngleShoveDegrees;
                remaining *= Mathf.Pow(Mathf.Abs(1 - normalizedAngle), collisionMsg.anglePower);
                Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

                if (projected.magnitude + collisionMsg.epsilon < remaining.magnitude)
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
        ///  上下楼梯贴地
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3 SnapPlayerDown(Vector3 position)
        {
            var rotation = entity.GetWorldRotate().Value;
            bool hit = CastSelf(
                position,
                rotation,
                Vector3.down,
                collisionMsg.stepUpDepth,
                out RaycastHit groundHit,
                collisionMsg.MaskLayer,
                collisionMsg.skinWidth);
            if (hit && groundHit.distance > (collisionMsg.groundDist + collisionMsg.skinWidth))
            {
                position += Vector3.down * groundHit.distance + new Vector3(0, collisionMsg.groundDist - collisionMsg.epsilon, 0);
            }

            return position;
        }

        private void Clear()
        {
        }
    }
}