using UnityEngine;

namespace GXGame
{
    public partial class CollisionWorldPosDirSystem
    {
        private void UpdateMovingGround(Vector3 position, Quaternion rotation)
        {
            var value = entity.GetPreviousGroundMsgComponent().Value;
            if (!groundMsg.OnGround)
            {
                value.RelativePos = Vector3.zero;
                value.RelativeRotation = Quaternion.identity;
                value.PreviousParent = null;
                return;
            }

            var parent = groundMsg.RaycastHit.transform;
            var parentInverse = Quaternion.Inverse(parent.rotation);
            //得到我在父物体中的本地旋转
            value.RelativeRotation = rotation * parentInverse;
            //得到我在父物体中的本地坐标.
            value.RelativePos = position - parent.position;
            value.RelativePos = parentInverse * value.RelativePos;
            value.PreviousParent = parent;
        }


        private Quaternion DeltaRotation(Quaternion a)
        {
            var value = entity.GetPreviousGroundMsgComponent().Value;
            if (!groundMsg.OnGround || value.PreviousParent == null)
            {
                return a;
            }

            var rotation = value.PreviousParent.rotation * value.RelativeRotation;
            rotation.x = 0;
            rotation.z = 0;
            return rotation;
        }

        private Vector3 DeltaPosition(Vector3 position)
        {
            var value = entity.GetPreviousGroundMsgComponent().Value;
            if (!groundMsg.OnGround || value.PreviousParent == null)
            {
                return Vector3.zero;
            }

            return (value.PreviousParent.position + value.PreviousParent.rotation * value.RelativePos) - position;
        }

        private void FollowGround(ref Vector3 position, ref Quaternion rotation)
        {
            position += DeltaPosition(position);
            rotation = DeltaRotation(rotation);
        }
    }
}