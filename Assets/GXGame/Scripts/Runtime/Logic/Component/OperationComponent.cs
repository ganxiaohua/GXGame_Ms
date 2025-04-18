﻿using GameFrame;
using UnityEngine;

namespace GXGame
{
    public struct Operation
    {
        public Vector2 MoveDir;
        public Vector2 CameraDir;
        public bool Jump;
        public bool Interaction;
        public bool Throw;
        public ECSEntity OperationTarget;
    }

    public class OperationComponent : ECSComponent
    {
        public Operation Value;
    }
}