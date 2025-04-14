using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class BeingCaughtData
    {
        /// <summary>
        /// 和持有者差距
        /// </summary>
        public Vector3 Offset;

        /// <summary>
        /// 持有者
        /// </summary>
        public ECSEntity Holder;
    }

    /// <summary>
    /// 被抓住
    /// </summary>
    public class BeingCaughtComponent : ECSComponent
    {
        public BeingCaughtData Value;
    }

    /// <summary>
    /// 抓住
    /// </summary>
    public class CaughtComponent : ECSComponent
    {
        public ECSEntity Value;
    }
}