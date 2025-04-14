using System;
using System.Collections.Generic;
using Common.Runtime;
using GameFrame;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GXGame
{
    public class CapsuleColliderData : IDisposable
    {
        public GXGameObject Go;
        public UnityEngine.CapsuleCollider CapsuleCollider;

        public static CapsuleColliderData Create(ECSEntity ecsEntity, LayerMask layerMask)
        {
            CapsuleColliderData capsuleColliderData = ReferencePool.Acquire<CapsuleColliderData>();
            capsuleColliderData.Go = new GXGameObject();
            capsuleColliderData.Go.BindFromEmpty(Main.CollisionLayer);
            capsuleColliderData.Go.gameObject.name = ecsEntity.Name;
            capsuleColliderData.Go.gameObject.layer = layerMask;
            capsuleColliderData.CapsuleCollider = capsuleColliderData.Go.gameObject.AddComponent<UnityEngine.CapsuleCollider>();
            capsuleColliderData.Go.gameObject.AddComponent<CollisionEntity>().Entity = ecsEntity;
            capsuleColliderData.CapsuleCollider.radius = 0.31f;
            capsuleColliderData.CapsuleCollider.height = 1.79f;
            capsuleColliderData.CapsuleCollider.center = new Vector3(0, 0.92f, 0);
            capsuleColliderData.Go.position = ecsEntity.GetWorldPos().Value;
            return capsuleColliderData;
        }

        public static void Release(CapsuleColliderData capsuleColliderData)
        {
            ReferencePool.Release(capsuleColliderData);
        }

        public void Dispose()
        {
            var box = Go.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            Object.Destroy(box);
            var entity = Go.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            Go.Unbind();
            Go = null;
        }
    }

    /// <summary>
    /// 由于unity自身碰撞逻辑和渲染并不分离这个组件并不典型.
    /// </summary>
    public class CapsuleColliderComponent : ECSComponent
    {
        public CapsuleColliderData Value;

        public override void Dispose()
        {
            CapsuleColliderData.Release(Value);
        }
    }
    /**********************************************************************************/
    //Box

    public class BoxColliderData : IDisposable
    {
        public GXGameObject Go;
        public UnityEngine.BoxCollider BoxCollider;

        public static BoxColliderData Create(ECSEntity ecsEntity, LayerMask layerMask)
        {
            BoxColliderData boxData = ReferencePool.Acquire<BoxColliderData>();
            boxData.Go = new GXGameObject();
            boxData.Go.BindFromEmpty(Main.CollisionLayer);
            boxData.Go.gameObject.name = ecsEntity.Name;
            boxData.Go.gameObject.layer = layerMask;
            boxData.BoxCollider = boxData.Go.gameObject.AddComponent<UnityEngine.BoxCollider>();
            boxData.Go.gameObject.AddComponent<CollisionEntity>().Entity = ecsEntity;
            var scale = ecsEntity.GetLocalScale().Value;
            boxData.BoxCollider.center = new Vector3(0, scale.y / 2, 0);
            boxData.BoxCollider.size = scale;
            boxData.Go.position = ecsEntity.GetWorldPos().Value;
            return boxData;
        }

        public static void Release(BoxColliderData doxColliderData)
        {
            ReferencePool.Release(doxColliderData);
        }

        public void Dispose()
        {
            var box = Go.gameObject.GetComponent<UnityEngine.BoxCollider>();
            Object.Destroy(box);
            var entity = Go.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            Go.Unbind();
            Go = null;
        }
    }


    public class BoxColliderComponent : ECSComponent
    {
        public BoxColliderData Value;


        public override void Dispose()
        {
            BoxColliderData.Release(Value);
        }
    }


    public class RaycastHitMsg : ECSComponent
    {
        public List<RaycastHit> Value;
    }


    /// <summary>
    /// 碰到地图碰撞体的行为
    /// </summary>
    public class CollisionGroundType : ECSComponent
    {
        public int Type = 0;

        /// <summary>
        /// 滑行
        /// </summary>
        public const int Slide = 0;

        /// <summary>
        /// 弹开
        /// </summary>
        public const int Bomb = 1;

        /// <summary>
        /// 反射
        /// </summary>
        public const int Reflect = 2;
    }
}