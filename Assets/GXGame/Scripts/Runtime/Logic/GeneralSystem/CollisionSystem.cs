using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 碰撞系统
    /// </summary>
    public partial class CollisionSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private RaycastHit[] raycastHit = new RaycastHit[4];
        private RaycastHit[] raycastHitFloor = new RaycastHit[4];
        private List<RaycastHit> collisionWithObjectLayer;
        private List<RaycastHit> collisionWithWallLayer;
        private Group group;
        private World world;
        private bool isfloor = false;

        public void OnInitialize(World entity)
        {
            world = entity;
            collisionWithObjectLayer = new List<RaycastHit>();
            collisionWithWallLayer = new List<RaycastHit>();
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.FaceDirection, Components.FaceDirection, Components.CapsuleCollider);
            group = entity.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                var capsuleCollider = entity.GetCapsuleCollider();
                SetWolrdPos(entity, capsuleCollider);
                SetWorldRotate(entity, capsuleCollider);
                entity.SetCapsuleCollider(capsuleCollider.Value);
            }
        }

        private void SetWolrdPos(ECSEntity entity, CapsuleCollider capsuleCollider)
        {
            var dir = entity.GetMoveDirection().Value;
            var pos = entity.GetWorldPos().Value;
            var speed = entity.GetMoveSpeed().Value;
            dir = (new Vector3(dir.x, 0, dir.z).normalized * speed + new Vector3(0, -1, 0)) * Time.deltaTime;
            var distance = dir.magnitude;
            dir = dir.normalized;
            CollisionTest(pos, dir, distance, entity);
            if (IsFloor())
            {
                //如果有碰触到地面,则将向下的距离去掉,同时修正dir
                dir = new Vector3(dir.x, 0, dir.z).normalized * speed* Time.deltaTime;
                distance = dir.magnitude;
                dir = dir.normalized;
            }
            ObjectCollision(entity);
            dir = GetCollisionDir(entity, dir);
            //以下验算新的dir是否会碰撞
            if (IsResetDir())
            {
                CollisionTest(pos, dir, distance, entity);
            }

            if (IsResetDir())
            {
                return;
            }

            ObjectCollision(entity);
            //新的方向没有碰撞,执行位置更换
            pos += dir * distance;
            capsuleCollider.Value.position = pos;
            entity.SetWorldPos(pos);
        }

        private void SetWorldRotate(ECSEntity entity, CapsuleCollider capsuleCollider)
        {
            var dir = entity.GetFaceDirection().Value;
            float speed = entity.GetDirectionSpeed().Value;
            Vector3 nowDir = capsuleCollider.Value.rotation * Vector3.forward;
            float angle = speed * Time.deltaTime * world.Multiple;
            var curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            var drot = Quaternion.LookRotation(curDir);
            capsuleCollider.Value.rotation = drot;
            entity.SetWorldRotate(drot);
        }

        private void CollisionTest(Vector3 pos, Vector3 dir, float distance, ECSEntity entity)
        {
            collisionWithObjectLayer.Clear();
            collisionWithWallLayer.Clear();
            var box = entity.GetCapsuleCollider();
            var count = BoxCast(pos, dir, distance, box);
            if (count == 0) return;
            CollisionSeparation(entity, count);
        }

        private Vector3 GetCollisionDir(ECSEntity entity, Vector3 dir)
        {
            if (!IsResetDir())
            {
                return dir;
            }

            return WallCollision(dir, entity);
        }

        private int BoxCast(Vector3 pos, Vector3 dir, float distance, CapsuleCollider box)
        {
            var collider = box.Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            float offset = collider.height / 2;
            Vector3 start = pos - new Vector3(0, offset, 0) + collider.center;
            Vector3 end = pos + new Vector3(0, offset, 0) + collider.center;
            Vector3 startFloor = new Vector3(start.x, start.y - Time.deltaTime * 1 - 0.01f, start.z);
            Vector3 endFloor = new Vector3(end.x, end.y - Time.deltaTime * 1 - 0.01f, end.z);
            int count = Physics.CapsuleCastNonAlloc(start, end, collider.radius, dir, raycastHit, distance);
            int count2 = Physics.CapsuleCastNonAlloc(startFloor, endFloor, collider.radius - 0.01f, dir, raycastHitFloor, distance);
            for (int i = 0; i < count; i++)
            {
                //过滤自己
                if (raycastHit[i].transform == box.Value.transform)
                {
                    raycastHit[i] = raycastHit[count - 1];
                    raycastHit[count - 1] = default;
                    count--;
                    break;
                }
            }

            for (int i = 0; i < count2; i++)
            {
                //过滤自己
                if (raycastHitFloor[i].transform == box.Value.transform)
                {
                    raycastHitFloor[i] = raycastHitFloor[count2 - 1];
                    raycastHitFloor[count2 - 1] = default;
                    count2--;
                    break;
                }
            }
            
            isfloor = count2 > 0;
            return count;
        }

        /// <summary>
        /// 碰触优先级,碰到其他东西的优先级 并且过滤掉同阵营的
        /// </summary>
        /// <returns></returns>
        private void CollisionSeparation(ECSEntity owner, int count)
        {
            var camp = owner.GetCampComponent().Value;
            for (int i = 0; i < count; i++)
            {
                if (raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Object"))
                {
                    //过滤掉同阵营
                    var rayCamp = raycastHit[i].transform.GetComponent<CollisionEntity>().Entity.GetCampComponent();
                    if (rayCamp != null && rayCamp.Value == camp)
                    {
                        continue;
                    }

                    collisionWithObjectLayer.Add(raycastHit[i]);
                }
                else if (raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Wall"))
                {
                    collisionWithWallLayer.Add(raycastHit[i]);
                }
            }
        }

        private bool IsResetDir()
        {
            if (collisionWithWallLayer.Count > 0)
                return true;
            return false;
        }

        private bool IsFloor()
        {
            return isfloor;
        }

        private void ObjectCollision(ECSEntity entity)
        {
            if (collisionWithObjectLayer.Count != 0)
            {
                var hit = entity.GetRaycastHitMsg();
                if (hit == null)
                {
                    entity.AddRaycastHitMsg(new List<RaycastHit>());
                    hit = entity.GetRaycastHitMsg();
                }

                foreach (var t in collisionWithObjectLayer)
                {
                    hit.Value.Add(t);
                }

                entity.SetRaycastHitMsg(hit.Value);
            }
        }

        private Vector3 WallCollision(Vector3 dir, ECSEntity entity)
        {
            if (collisionWithWallLayer.Count == 0)
                return dir;
            Vector3 normal = Vector3.zero;
            foreach (var collision in collisionWithWallLayer)
            {
                normal += collision.normal;
            }

            if ((entity.GetCollisionGroundType().Type == CollisionGroundType.Slide))
            {
                var rayNormal = normal.normalized;
                Vector3 projection = Vector3.Dot(-dir, rayNormal) * rayNormal;
                dir = (dir + projection).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Reflect)
            {
                dir = Vector3.Reflect(dir, normal).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Bomb)
            {
                dir = -dir;
            }

            return dir;
        }


        public void Dispose()
        {
            collisionWithObjectLayer.Clear();
            collisionWithWallLayer.Clear();
        }
    }
}