using UnityEngine;

namespace GXGame
{
    public partial class ControlSystem
    {

        private void UpdateMovingGround(Vector3 position, Quaternion rotation)
        {
            var value = entity.GetGroundMsgComponent().Value;
            if (!groundMsg.onGround)
            {
                value.RelativePos = Vector3.zero;
                value.RelativeRotation = Quaternion.identity;
                value.PreviousParent = null;
                return;
            }

            var parent = groundMsg.hit.transform;
            var parentInverse = Quaternion.Inverse(parent.rotation);
            //得到我在父物体中的本地旋转
            value.RelativeRotation = rotation * parentInverse;
            value.RelativePos = position  - parent.position;
            //得到我在父物体中的本地坐标.
            value.RelativePos = parentInverse * value.RelativePos;
            value.PreviousParent = parent;
        }


        private Quaternion DeltaRotation(Quaternion a)
        {
            var value = entity.GetGroundMsgComponent().Value;
            if (!groundMsg.onGround || value.PreviousParent == null)
            {
                return a;
            }

            var x = value.PreviousParent.rotation * value.RelativeRotation;
            x.x = 0;
            x.z = 0;
            return x;
        }

        private Vector3 DeltaPosition(Vector3 position)
        {
            var value = entity.GetGroundMsgComponent().Value;
            if (!groundMsg.onGround || value.PreviousParent == null)
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