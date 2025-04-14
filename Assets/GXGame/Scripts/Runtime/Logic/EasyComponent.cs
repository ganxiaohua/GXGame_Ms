using GameFrame;
using UnityEngine;

namespace GXGame
{
    public static class EasyComponent
    {
        /// <summary>
        /// 加入受到重力影响的效果
        /// </summary>
        /// <param name="entity"></param>
        public static void AddBoxCollisionComponent(ECSEntity entity)
        {
            Assert.IsTrue(entity != null && entity.IsAction, "这个实体不合法");
            entity.AddGravityComponent(24);
            entity.AddPreviousGroundMsgComponent(new PreviousGroundMsg());
            entity.AddGroundCollisionComponent(new GroundCollision());
            entity.AddCollisionMsgComponent(new CollisionMsg()
            {
                MaskLayer = ~0 //~(1 << LayerMask.NameToLayer("Interaction") | 1 << LayerMask.NameToLayer("Monster"))
            });
            entity.AddBoxColliderComponent(BoxColliderData.Create(entity, LayerMask.NameToLayer($"NoInteraction")));
        }
    }
}