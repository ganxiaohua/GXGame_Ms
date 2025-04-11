using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class PreviousGroundMsg
    {
        public Vector3 RelativePos;
        public Quaternion RelativeRotation;
        public Transform PreviousParent;
    }


    public class PreviousGroundMsgComponent : ECSComponent
    {
        public PreviousGroundMsg Value;
    }

    public class GroundCollision
    {
        public bool OnGround;
        public Vector3 OroundAngle;
        public RaycastHit RaycastHit;
    }


    public class GroundCollisionComponent : ECSComponent
    {
        public GroundCollision Value;
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
        public int MaskLayer;
    }

    public class CollisionMsgComponent : ECSComponent
    {
        public CollisionMsg Value;
    }

    public class GravityComponent : ECSComponent
    {
        public float Value;
    }
}