using UnityEngine;

namespace GXGame
{
    public partial class CollisionWorldPosDirSystem
    {
        private bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit, int layerMask, float skinWidth = 0.01f)
        {
            if (entity.HasComponent(Components.CapsuleColliderComponent))
            {
                var capsuleColliderComponent = entity.GetCapsuleColliderComponent().Value;
                return CollisionDetection.CapsuleCastNonAlloc(capsuleColliderComponent.Go.transform, raycastHit,
                    capsuleColliderComponent.CapsuleCollider, pos, rot, dir, dist,
                    out hit,
                    layerMask,
                    skinWidth);
            }
            else if (entity.HasComponent(Components.BoxColliderComponent))
            {
                var boxColliderComponent = entity.GetBoxColliderComponent().Value;
                return CollisionDetection.BoxCastNonAlloc(boxColliderComponent.Go.transform, raycastHit, pos, rot, dir, dist,
                    boxColliderComponent.BoxCollider.size,
                    out hit, layerMask);
            }

            Debug.LogError("错误的分支");
            hit = default;
            return false;
        }


        private bool CheckPerpendicularBounce(
            RaycastHit hit,
            Vector3 momentum)
        {
            bool hitStep = CollisionDetection.Raycast(
                hit.point - Vector3.up * collisionMsg.epsilon + hit.normal * collisionMsg.epsilon,
                momentum.normalized,
                momentum.magnitude,
                out RaycastHit stepHit,
                collisionMsg.MaskLayer,
                QueryTriggerInteraction.Ignore);
            return hitStep &&
                   Vector3.Dot(stepHit.normal, Vector3.up) <= collisionMsg.epsilon;
        }

        Vector3 GetBottom(Vector3 position, Quaternion rotation)
        {
            if (entity.HasComponent(Components.CapsuleColliderComponent))
            {
                var capsuleColliderComponent = entity.GetCapsuleColliderComponent();
                var ccc = CollisionDetection.CalculateCapsuleCollider(capsuleColliderComponent.Value.CapsuleCollider, position, rotation, 0);
                return ccc.bottom + ccc.radius * (rotation * -Vector3.up);
            }
            else if (entity.HasComponent(Components.BoxColliderComponent))
            {
                var boxColliderComponent = entity.GetBoxColliderComponent().Value;
                return position - new Vector3(0, boxColliderComponent.BoxCollider.size.y / 2, 0);
            }

            Debug.LogError("错误的分支");
            return position;
        }

        private bool AttemptSnapUp(
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
                collisionMsg.MaskLayer,
                collisionMsg.skinWidth);
            if (!didSnapHit)
            {
                float distanceMove = Mathf.Min(momentum.magnitude, distanceToSnap);
                position += distanceMove * Vector3.up;
                return true;
            }

            return false;
        }


        private bool AttemptSnapUp(
            RaycastHit hit,
            ref Vector3 momentum,
            ref Vector3 position,
            Quaternion rotation)
        {
            Vector3 bottom = GetBottom(position, rotation);
            Vector3 footVector = Vector3.Project(hit.point, Vector3.up) - Vector3.Project(bottom, Vector3.up);
            bool isAbove = Vector3.Dot(footVector, Vector3.up) > 0;
            float distanceToFeet = footVector.magnitude * (isAbove ? 1 : -1);
            bool snappedUp = false;
            if (distanceToFeet < collisionMsg.stepUpDepth)
            {
                snappedUp = AttemptSnapUp(
                    distanceToFeet,
                    ref momentum,
                    ref position,
                    rotation);
                //操作指令方向爬不上就设置攀爬高度在尝试一下
                if (!snappedUp)
                {
                    snappedUp = AttemptSnapUp(
                        collisionMsg.stepUpDepth,
                        ref momentum,
                        ref position,
                        rotation);
                }
            }

            return snappedUp;
        }
    }
}