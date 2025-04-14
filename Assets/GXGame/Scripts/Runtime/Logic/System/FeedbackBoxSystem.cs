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

        private CapsuleColliderComponent capsuleColliderComponent;


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
                capsuleColliderComponent = entity.GetCapsuleColliderComponent();
                var pos = this.entity.GetWorldPos().Value;
                var rot = this.entity.GetWorldRotate().Value;
                var feedBackBoxComp = this.entity.GetFeedBackBoxComponent().Value;
                int overlappingCount = CollisionDetection.OverlapBoxNonAlloc(capsuleColliderComponent.Value.Go.transform, colliders,
                    rot * Vector3.forward + pos,
                    rot,
                    feedBackBoxComp.Size,
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