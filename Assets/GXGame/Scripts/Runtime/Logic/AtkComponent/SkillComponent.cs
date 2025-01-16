﻿using GameFrame;

namespace GXGame
{
    [ViewBind(typeof(IAtk))]
    public class PlayAtk : ECSComponent
    {
        public int Id;
    }

    /// <summary>
    /// 技能前摇
    /// </summary>
    public class PreSkillComponent : ECSComponent
    {
        public float Time;
    }


    public class LateSkillComponent : ECSComponent
    {
        public float Time;
    }

    /// <summary>
    /// 释放技能的间隔
    /// </summary>
    public class AtkIntervalComponent : ECSComponent
    {
        public float Time;
    }

    [AssignBind(typeof(SkillEntity))]
    public class SkillComponent : ECSComponent
    {
        public int ID;
    }

    /// <summary>
    /// 技能作用的角色类型
    /// </summary>
    [AssignBind(typeof(SkillEntity))]
    public class AbilityUnitTypeComponent : ECSComponent
    {
        public UnitTypeEnum AbilityUnitTargetTeam;
    }
}