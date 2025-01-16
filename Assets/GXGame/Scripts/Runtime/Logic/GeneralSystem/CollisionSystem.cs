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

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate, Components.MoveDirection);

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
                var collisionBox = entity.GetCollisionBox();
                collisionBox.Value.position = pos;
                entity.SetCollisionBox(collisionBox.Value);
            }
        }

        private Vector3 GetCollisionDir(Vector3 pos, Vector3 dir, float distance, ECSEntity entity)
        {
            var box = entity.GetCollisionBox();
            var count = BoxCast(pos, dir, distance, box);
            if (count == 0) return dir;
            var collisonProority = CollisionPriority(entity, count);
            RaycastHit targetRaycastHit2D = collisonProority.hit;
            if (collisonProority.priority == 0)
            {
                return dir;
            }
            else if (collisonProority.priority == 2)
            {
                var hit = entity.GetRaycastHitMsg();
                if (hit == null)
                {
                    entity.AddRaycastHitMsg(new List<RaycastHit>());
                    hit = entity.GetRaycastHitMsg();
                }

                hit.Value.Add(targetRaycastHit2D);
                entity.SetRaycastHitMsg(hit.Value);
                return Vector2.zero;
            }

            if ((entity.GetCollisionGroundType().Type == CollisionGroundType.Slide))
            {
                var rayNormal = targetRaycastHit2D.normal.normalized;
                Vector3 projection = Vector3.Dot(-dir, rayNormal) / rayNormal.sqrMagnitude * rayNormal;
                dir = (dir + projection).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Reflect)
            {
                dir = Vector3.Reflect(dir, targetRaycastHit2D.normal).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Bomb)
            {
                dir = -dir;
            }

            return dir;
        }

        private int BoxCast(Vector3 pos, Vector3 dir, float distance, CollisionBox box)
        {
            var collider = box.Value.gameObject.GetComponent<CapsuleCollider>();
            float offset = collider.height / 2;
            Vector3 start = pos - new Vector3(0, offset, 0);
            Vector3 end = pos + new Vector3(0, offset, 0);
            int count = Physics.CapsuleCastNonAlloc(start, end, collider.radius, dir, raycastHit, distance);
            for (int i = 0; i < count; i++)
            {
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
        private (int priority, RaycastHit hit) CollisionPriority(ECSEntity owner, int count)
        {
            int priority = 0;
            RaycastHit hit2D = default;
            var camp = owner.GetCampComponent().Value;
            for (int i = 0; i < count; i++)
            {
                if (priority < 2 && raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Object"))
                {
                    //过滤掉同阵营
                    var rayCamp = raycastHit[i].transform.GetComponent<CollisionEntity>().Entity.GetCampComponent();
                    if (rayCamp != null && rayCamp.Value == camp)
                    {
                        continue;
                    }
                    priority = 2;
                    hit2D = raycastHit[i];
                }
                else if (priority < 1 && raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Wall"))
                {
                    priority = 1;
                    hit2D = raycastHit[i];
                }
            }

            return (priority, hit2D);
        }


        public override void Dispose()
        {
        }
    }
}