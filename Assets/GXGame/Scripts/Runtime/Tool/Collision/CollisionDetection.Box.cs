using System;
using UnityEngine;

namespace GXGame
{
    public static partial class CollisionDetection
    {
        public static int OverlapBoxNonAlloc(
            Transform selfCollider,
            Collider[] colliders,
            Vector3 position,
            Quaternion rotation,
            Vector3 size,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var pos = rotation * Vector3.forward + position;
            Array.Clear(colliders, 0, colliders.Length);
            int overlap = Physics.OverlapBoxNonAlloc(pos, size / 2, colliders, rotation, layerMask, queryTriggerInteraction);
#if UNITY_EDITOR
            SetBox(pos, rotation, size);
#endif
            for (int i = 0; i < overlap; i++)
            {
                var tempHit = colliders[i];
                if (tempHit.transform == selfCollider)
                {
                    colliders[i] = default;
                    colliders[i] = colliders[overlap - 1];
                    overlap--;
                    break;
                }
            }

            return overlap;
        }

        public static bool Raycast(Vector3 source, Vector3 direction, float distance, out RaycastHit stepHit, int layerMask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            bool didHit = Physics.Raycast(new Ray(source, direction), out stepHit, distance, layerMask, queryTriggerInteraction);
            return didHit;
        }
    }
}