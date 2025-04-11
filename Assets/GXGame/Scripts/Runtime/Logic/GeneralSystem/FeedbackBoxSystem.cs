using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class FeedbackBoxSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private World world;

        private Collider[] colliders;

        private ECSEntity entity;

        private Group group;

        private CapsuleCollider capsuleCollider;


        public void OnInitialize(World world)
        {
            this.world = world;
            colliders = new Collider[10];
            Matcher matcher = Matcher.SetAll(Components.FeedBackBoxComponent);
            group = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                this.entity = entity;
                capsuleCollider = entity.GetCapsuleCollider();
                var pos = this.entity.GetWorldPos().Value;
                var rot = this.entity.GetWorldRotate().Value;
                var feedBackBoxComp = this.entity.GetFeedBackBoxComponent().Value;
                int overlappingCount = CollisionDetection.OverlapBoxNonAlloc(capsuleCollider.Value.transform, colliders, pos, rot, feedBackBoxComp.Size,
                    feedBackBoxComp.MaskLayer);
                feedBackBoxComp.FeedBackEntitys ??= new();
                feedBackBoxComp.FeedBackEntitys.Clear();
                if (overlappingCount != 0)
                {
                    for (int i = 0; i < overlappingCount; i++)
                    {
                        feedBackBoxComp.FeedBackEntitys.Add(colliders[i].transform.GetComponent<CollisionEntity>().Entity);
                    }
                }
            }
        }


        public void Dispose()
        {
        }
    }
}