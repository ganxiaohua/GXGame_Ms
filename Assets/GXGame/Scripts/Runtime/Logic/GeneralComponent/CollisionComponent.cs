using System.Collections.Generic;
using Common.Runtime;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 由于unity自身碰撞逻辑和渲染并不分离这个组件并不典型.
    /// </summary>
    public class CapsuleCollider : ECSComponent
    {
        public GXGameObject Value;

        public static GXGameObject Create(ECSEntity ecsEntity, LayerMask layerMask)
        {
            var value = new GXGameObject();
            value.BindFromEmpty(Main.CollisionLayer);
            value.gameObject.name = ecsEntity.Name;
            value.gameObject.layer = layerMask;
            var collider = value.gameObject.AddComponent<UnityEngine.CapsuleCollider>();
            value.gameObject.AddComponent<CollisionEntity>().Entity = ecsEntity;
            collider.radius = 0.31f;
            collider.height = 1.79f;
            collider.center = new Vector3(0, 0.92f, 0);
            value.position = ecsEntity.GetWorldPos().Value;
            return value;
        }

        public override void Dispose()
        {
            var box = Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            Object.Destroy(box);
            var entity = Value.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            Value.Unbind();
            Value = null;
        }
    }

    public class BoxCollider : ECSComponent
    {
        public GXGameObject Value;

        public static GXGameObject Create(ECSEntity ecsEntity, LayerMask layerMask)
        {
            var value = new GXGameObject();
            value.BindFromEmpty(Main.CollisionLayer);
            value.gameObject.name = ecsEntity.Name;
            value.gameObject.layer = layerMask;
            var collider = value.gameObject.AddComponent<UnityEngine.BoxCollider>();
            value.gameObject.AddComponent<CollisionEntity>().Entity = ecsEntity;
            var scale = ecsEntity.GetLocalScale().Value;
            collider.center = new Vector3(0, scale.y / 2, 0);
            collider.size = scale;
            value.position = ecsEntity.GetWorldPos().Value;
            return value;
        }

        public override void Dispose()
        {
            var box = Value.gameObject.GetComponent<UnityEngine.CapsuleCollider>();
            Object.Destroy(box);
            var entity = Value.gameObject.GetComponent<CollisionEntity>();
            Object.Destroy(entity);
            Value.Unbind();
            Value = null;
        }
    }

    public class YAxisAcceleration : ECSComponent
    {
        public bool Value;
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