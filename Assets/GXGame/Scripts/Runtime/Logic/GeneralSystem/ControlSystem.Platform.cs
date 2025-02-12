using UnityEngine;

namespace GXGame
{
    public partial class ControlSystem
    {
        public Vector3 RelativePos;
        public Quaternion RelativeRotation;
        public Transform PreviousParent { get; internal set; }

        public void UpdateMovingGround(Vector3 position, Quaternion rotation, Vector3 delta)
        {
            if (!groundMsg.onGround)
            {
                Reset();
                return;
            }

            var parent = groundMsg.hit.transform;
            RelativeRotation = rotation * Quaternion.Inverse(parent.rotation);
            if (parent != PreviousParent)
            {
                RelativePos = position + delta - parent.position;
                RelativePos = Quaternion.Inverse(parent.rotation) * RelativePos;
            }
            else
            {
                RelativePos += Quaternion.Inverse(parent.rotation) * delta;
            }

            PreviousParent = parent;
        }


        public Quaternion DeltaRotation(Quaternion a)
        {
            if (!groundMsg.onGround || PreviousParent == null)
            {
                return a;
            }

            return PreviousParent.rotation * Quaternion.Inverse(RelativeRotation);
        }

        public Vector3 DeltaPosition(Vector3 position)
        {
            if (!groundMsg.onGround || PreviousParent == null)
            {
                return Vector3.zero;
            }

            return (PreviousParent.position + PreviousParent.rotation * RelativePos) - position;
        }

        public virtual void FollowGround(ref Vector3 position, ref Quaternion rotation)
        {
            position += DeltaPosition(position);
            // rotation = DeltaRotation(rotation);
        }


        public void Reset()
        {
            RelativePos = Vector3.zero;
            RelativeRotation = Quaternion.identity;
            PreviousParent = null;
        }
    }
}