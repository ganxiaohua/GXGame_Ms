using UnityEngine;

namespace GXGame
{
    public partial class ControlSystem
    {
        public (Vector3 center, float radius, float height, Vector3 bottom, Vector3 top) CalculateCapsuleCollider(Vector3 position, Quaternion rotation,
            float flared)
        {
            Vector3 center = rotation * unityCapsuleCollider.center + position;
            float radius = unityCapsuleCollider.radius + flared;
            float height = unityCapsuleCollider.height + flared * 2;
            Vector3 bottom = center + rotation * Vector3.down * (height / 2 - radius);
            Vector3 top = center + rotation * Vector3.up * (height / 2 - radius);
            return (center, radius, height, bottom, top);
        }

        public int GetOverlapping(
            Vector3 position,
            Quaternion rotation,
            int layerMask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore,
            float skinWidth = 0.0f)
        {
            var ccc = CalculateCapsuleCollider(position, rotation, -skinWidth);
            int overlap = Physics.OverlapCapsuleNonAlloc(ccc.top, ccc.bottom, ccc.radius, OverlapCache, layerMask, queryTriggerInteraction);
            for (int i = 0; i < overlap; i++)
            {
                var tempHit = OverlapCache[i];
                //过滤自己
                if (tempHit.transform == capsuleCollider.Value.transform)
                {
                    OverlapCache[i] = default;
                    OverlapCache[i] = OverlapCache[overlap - 1];
                    overlap--;
                    break;
                }
            }

            return overlap;
        }

        public bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit, float skinWidth = 0.01f)
        {
            var ccc = CalculateCapsuleCollider(pos, rot, -skinWidth);
            int count = Physics.CapsuleCastNonAlloc(ccc.top, ccc.bottom, ccc.radius, dir, raycastHit, dist + skinWidth, ~0, QueryTriggerInteraction.Ignore);
            float directDist = float.MaxValue;
            bool didHit = false;
            hit = default;
            for (int i = 0; i < count; i++)
            {
                var tempHit = raycastHit[i];
                //过滤自己 选出距离最近的
                if (tempHit.transform != capsuleCollider.Value.transform && directDist > tempHit.distance)
                {
                    hit = tempHit;
                    directDist = tempHit.distance;
                    didHit = true;
                }
            }

            return didHit;
        }
    }
}