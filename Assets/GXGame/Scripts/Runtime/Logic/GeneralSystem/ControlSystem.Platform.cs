using UnityEngine;

namespace GXGame
{
    public partial class ControlSystem
    {
        private Vector3 relativePos;
        private Quaternion relativeRotation;
        private Transform previousParent;

        private void UpdateMovingGround(Vector3 position, Quaternion rotation, Vector3 delta)
        {
            if (!groundMsg.onGround)
            {
                Reset();
                return;
            }

            var parent = groundMsg.hit.transform;
            relativeRotation = rotation * Quaternion.Inverse(parent.rotation);
            if (parent != previousParent)
            {
                relativePos = position + delta - parent.position;
                relativePos = Quaternion.Inverse(parent.rotation) * relativePos;
            }
            else
            {
                relativePos += Quaternion.Inverse(parent.rotation) * delta;
            }

            previousParent = parent;
        }


        private Quaternion DeltaRotation(Quaternion a)
        {
            if (!groundMsg.onGround || previousParent == null)
            {
                return a;
            }

            return previousParent.rotation * relativeRotation;
        }

        private Vector3 DeltaPosition(Vector3 position)
        {
            if (!groundMsg.onGround || previousParent == null)
            {
                return Vector3.zero;
            }

            return (previousParent.position + previousParent.rotation * relativePos) - position;
        }

        private void FollowGround(ref Vector3 position, ref Quaternion rotation)
        {
            position += DeltaPosition(position);
            rotation = DeltaRotation(rotation);
        }


        private void Reset()
        {
            relativePos = Vector3.zero;
            relativeRotation = Quaternion.identity;
            previousParent = null;
        }
    }
}