using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class GroudMsg
    {
        public Vector3 RelativePos;
        public Quaternion RelativeRotation;
        public Transform PreviousParent;
    }


    public class GroundMsgComponent : ECSComponent
    {
        public GroudMsg Value;
    }

    public class CollisionMsg
    {
        public float groundDist = 0.01f;
        public float epsilon = 0.001f;
        public float skinWidth = 0.01f;
        public float anglePower = 2.0f;
        public float maxWalkingAngle = 60f;
        public float maxJumpAngle = 70f;
        public float jumpAngleWeightFactor = 0.1f;
        public float stepUpDepth = 0.5f;
        public float Gravity = 12;
        public int MaskLayer;
    }

    public class CollisionMsgComponent : ECSComponent
    {
        public CollisionMsg Value;
    }
}