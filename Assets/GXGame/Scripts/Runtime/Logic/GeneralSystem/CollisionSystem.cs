using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 碰到了就修正方向
    /// </summary>
    public partial class CollisionSystem : FixedUpdateReactiveSystem
    {
        private RaycastHit[] raycastHit = new RaycastHit[4];
        private List<RaycastHit> collisionWithObjectLayer;
        private List<RaycastHit> collisionWithWallLayer;

        public override void OnInitialize(World entity)
        {
            base.OnInitialize(entity);
            collisionWithObjectLayer = new List<RaycastHit>();
            collisionWithWallLayer = new List<RaycastHit>();
        }

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate, Components.MoveDirection, Components.FaceDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.CollisionBox) &&
                   entity.HasComponent(Components.MoveDirection) && entity.HasComponent(Components.MoveSpeed) && entity.HasComponent(Components.WorldPos);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Value;
                if (dir == Vector3.zero)
                    continue;
                var distance = entity.GetMoveSpeed().Value * Time.deltaTime * World.Multiple;
                var pos = entity.GetWorldPos().Value;
                dir = dir.normalized;
                dir = GetCollisionDir(pos, dir, distance, entity);
                entity.SetMoveDirection(dir);
                pos += dir * distance;
                var capsuleCollider = entity.GetCollisionBox();
                capsuleCollider.Value.position = pos;
                Turn(entity, capsuleCollider);
                entity.SetCollisionBox(capsuleCollider.Value);
            }
        }

        private void Turn(ECSEntity entity, CapsuleCollider capsuleCollider)
        {
            var dir = entity.GetFaceDirection().Value;
            float speed = entity.GetDirectionSpeed().Value;
            Vector3 nowDir = capsuleCollider.Value.rotation * Vector3.forward;
            float angle = speed * Time.deltaTime * World.Multiple;
            var curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            capsuleCollider.Value.rotation = Quaternion.LookRotation(curDir);
        }

        private Vector3 GetCollisionDir(Vector3 pos, Vector3 dir, float distance, ECSEntity entity)
        {
            collisionWithObjectLayer.Clear();
            collisionWithWallLayer.Clear();
            var box = entity.GetCollisionBox();
            var count = BoxCast(pos, dir, distance, box);
            if (count == 0) return dir;
            var separation = CollisionSeparation(entity, count);
            if (!separation)
            {
                return dir;
            }

            ObjectCollision(entity);
            return WallCollision(dir,entity);
        }

        private int BoxCast(Vector3 pos, Vector3 dir, float distance, CapsuleCollider box)
        {
            var collider = box.Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            float offset = collider.height / 2;
            Vector3 start = pos - new Vector3(0, offset, 0) + collider.center;
            Vector3 end = pos + new Vector3(0, offset, 0) + collider.center;
            int count = Physics.CapsuleCastNonAlloc(start, end, collider.radius, dir, raycastHit, distance);
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

            return count;
        }

        /// <summary>
        /// 碰触优先级,碰到其他东西的优先级 并且过滤掉同阵营的
        /// </summary>
        /// <returns></returns>
        private bool CollisionSeparation(ECSEntity owner, int count)
        {
            bool separation = false;
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
                    separation = true;
                }
                else if (raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Wall"))
                {
                    collisionWithWallLayer.Add(raycastHit[i]);
                    separation = true;
                }
            }

            return separation;
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

        private Vector3 WallCollision(Vector3 dir,ECSEntity entity)
        {
            Vector3 normal = Vector3.zero;
            foreach (var collision in collisionWithWallLayer)
            {
                normal += collision.normal;
            }

            if ((entity.GetCollisionGroundType().Type == CollisionGroundType.Slide))
            {
                var rayNormal = normal.normalized;
                rayNormal.y = 0;
                Vector3 projection = Vector3.Dot(-dir, rayNormal) / rayNormal.sqrMagnitude * rayNormal;
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


        public override void Dispose()
        {
            collisionWithObjectLayer.Clear();
            collisionWithWallLayer.Clear();
        }
    }
}