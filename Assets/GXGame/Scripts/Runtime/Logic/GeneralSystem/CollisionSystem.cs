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
        private RaycastHit[] tempRaycastHit = new RaycastHit[4];
        private List<RaycastHit> collisionWithObjectLayer;
        private Group group;
        private World world;

        public void OnInitialize(World entity)
        {
            world = entity;
            Matcher matcher = Matcher.SetAll(Components.MoveDirection, Components.FaceDirection, Components.FaceDirection, Components.CapsuleCollider);
            group = entity.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                var capsuleCollider = entity.GetCapsuleCollider();
                SetWolrdPos2(entity, capsuleCollider);
                SetWorldRotate(entity, capsuleCollider);
                entity.SetCapsuleCollider(capsuleCollider.Value);
            }
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


        // /// <summary>
        // /// 碰触优先级,碰到其他东西的优先级 并且过滤掉同阵营的
        // /// </summary>
        // /// <returns></returns>
        // private void CollisionSeparation(ECSEntity owner, int count)
        // {
        //     var camp = owner.GetCampComponent().Value;
        //     for (int i = 0; i < count; i++)
        //     {
        //         if (raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Object"))
        //         {
        //             //过滤掉同阵营
        //             var rayCamp = raycastHit[i].transform.GetComponent<CollisionEntity>().Entity.GetCampComponent();
        //             if (rayCamp != null && rayCamp.Value == camp)
        //             {
        //                 continue;
        //             }
        //
        //             collisionWithObjectLayer.Add(raycastHit[i]);
        //         }
        //         else if (raycastHit[i].transform.gameObject.layer == LayerMask.NameToLayer($"Wall"))
        //         {
        //             collisionWithWallLayer.Add(raycastHit[i]);
        //         }
        //     }
        // }


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


        public void Dispose()
        {
            collisionWithObjectLayer.Clear();
        }
    }
}