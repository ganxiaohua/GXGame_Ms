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
            Array.Clear(colliders, 0, colliders.Length);
            int overlap = Physics.OverlapBoxNonAlloc(position, size / 2, colliders, rotation, layerMask, queryTriggerInteraction);
#if UNITY_EDITOR
            SetBox(position, rotation, size);
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

        public static bool BoxCastNonAlloc(Transform self, RaycastHit[] raycastHit,
            Vector3 position,
            Quaternion rotation,
            Vector3 dir,
            float distance,
            Vector3 size, out RaycastHit hit,
            int layerMask,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            Array.Clear(raycastHit, 0, raycastHit.Length);
            int count = Physics.BoxCastNonAlloc(position, size / 2, dir, raycastHit, rotation, distance, layerMask, queryTriggerInteraction);
            float directDist = float.MaxValue;
            bool didHit = false;
            hit = default;
            for (int i = 0; i < count; i++)
            {
                var tempHit = raycastHit[i];
                //过滤自己 选出距离最近的
                if (tempHit.transform != self && directDist > tempHit.distance)
                {
                    hit = tempHit;
                    directDist = tempHit.distance;
                    didHit = true;
                }
            }

            return didHit;
        }

        public static bool Raycast(Vector3 source, Vector3 direction, float distance, out RaycastHit stepHit, int layerMask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            bool didHit = Physics.Raycast(new Ray(source, direction), out stepHit, distance, layerMask, queryTriggerInteraction);
            return didHit;
        }
    }
}