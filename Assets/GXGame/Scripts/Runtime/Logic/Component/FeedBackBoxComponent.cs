using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class FeedBackBoxData
    {
        public Vector3 Size;
        public int MaskLayer;
        public List<ECSEntity> FeedBackEntitys;
    }

    public class FeedBackBoxComponent : ECSComponent
    {
        public FeedBackBoxData Value;
    }
}