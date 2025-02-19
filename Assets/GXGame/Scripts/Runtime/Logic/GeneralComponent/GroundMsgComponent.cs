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

    
    public class GroundMsgComponent: ECSComponent
    {
        public GroudMsg Value;
    }
}