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
#if UNITY_EDITOR
            OpenDuringSceneGui();
#endif
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
                int overlappingCount = GetOverlapping(pos, rot, feedBackBoxComp.Size, feedBackBoxComp.MaskLayer);
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

        private int GetOverlapping(
            Vector3 position,
            Quaternion rotation,
            Vector3 size,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var pos = rotation * Vector3.forward * 1 + position;
            int overlap = Physics.OverlapBoxNonAlloc(pos, size, colliders, rotation, layerMask, queryTriggerInteraction);
#if UNITY_EDITOR
            SetPosVector3(pos, rotation, size);
#endif
            for (int i = 0; i < overlap; i++)
            {
                var tempHit = colliders[i];
                //过滤自己
                if (tempHit.transform == capsuleCollider.Value.transform)
                {
                    colliders[i] = default;
                    colliders[i] = colliders[overlap - 1];
                    overlap--;
                    break;
                }
            }

            return overlap;
        }


        public void Dispose()
        {
#if UNITY_EDITOR
            CloseDuringSceneGui();
#endif
        }
    }
}