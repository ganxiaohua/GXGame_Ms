namespace GXGame
{
    /// <summary>
    /// 单位类型
    /// </summary>
    public enum UnitTypeEnum
    {
        BASIC = 1,

        /// <summary>
        /// 怪物
        /// </summary>
        Monster = 1 << 1,

        /// <summary>
        /// 作物
        /// </summary>
        Crop = 1 << 2,

        /// <summary>
        /// 动物
        /// </summary>
        Animal = 1 << 3,

        /// <summary>
        /// 动物制品
        /// </summary>
        AnimalProducts = 1 << 4,
    }
}