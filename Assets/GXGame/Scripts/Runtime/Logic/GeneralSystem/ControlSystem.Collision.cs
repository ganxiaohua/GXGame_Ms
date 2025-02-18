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

        public bool DoRaycastInDirection(Vector3 source, Vector3 direction, float distance, out RaycastHit stepHit, int layerMask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            bool didHit = Physics.Raycast(new Ray(source, direction), out stepHit, distance, layerMask, queryTriggerInteraction);
            return didHit;
        }

        public bool CheckPerpendicularBounce(
            RaycastHit hit,
            Vector3 momentum)
        {
            bool hitStep = DoRaycastInDirection(
                hit.point - Vector3.up * epsilon + hit.normal * epsilon,
                momentum.normalized,
                momentum.magnitude,
                out RaycastHit stepHit,
                ~0,
                QueryTriggerInteraction.Ignore);
            return hitStep &&
                   Vector3.Dot(stepHit.normal, Vector3.up) <= epsilon;
        }

        Vector3 GetBottom(Vector3 position, Quaternion rotation)
        {
            var ccc = CalculateCapsuleCollider(position, rotation, 0);
            return ccc.bottom + ccc.radius * (rotation * -Vector3.up);
        }
        
        public  bool AttemptSnapUp(
            float distanceToSnap,
            ref Vector3 momentum,
            ref Vector3 position,
            Quaternion rotation)
        {
            Vector3 snapPos = position + distanceToSnap * Vector3.up;
            bool didSnapHit = CastSelf(
                snapPos,
                rotation,
                momentum.normalized,
                momentum.magnitude,
                out RaycastHit snapHit,
                skinWidth);
            if (!didSnapHit)
            {
                float distanceMove = Mathf.Min(momentum.magnitude, distanceToSnap);
                position += distanceMove * Vector3.up;
                return true;
            }
            
            return false;
        }
        
        
        public  bool AttemptSnapUp(
            RaycastHit hit,
            ref Vector3 momentum,
            ref Vector3 position,
            Quaternion rotation)
        {

            Vector3 bottom = GetBottom(position, rotation);
            Vector3 footVector = Vector3.Project(hit.point,Vector3.up) - Vector3.Project(bottom, Vector3.up);
            bool isAbove = Vector3.Dot(footVector, Vector3.up) > 0;
            float distanceToFeet = footVector.magnitude * (isAbove ? 1 : -1);
            bool snappedUp = false;
            if (distanceToFeet < stepUpDepth)
            {
                snappedUp = AttemptSnapUp(
                    distanceToFeet,
                    ref momentum,
                    ref position,
                    rotation);
                
            }
            return snappedUp;
        }
    }
}