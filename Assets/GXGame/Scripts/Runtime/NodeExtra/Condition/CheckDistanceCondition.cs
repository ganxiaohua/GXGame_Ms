using System;
using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("AI")]
    [Description("判断和目标的距离")]
    public class CheckDistanceCondition : ConditionTask
    {
        private ECSEntity owner;
        private World world;

        public float MaxDistance;

        private Group playerGroup;

        public BBParameter<Int32> PlayerComponents;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            Matcher matcher = Matcher.SetAll(PlayerComponents.value);
            playerGroup = world.GetGroup(matcher);
            return null;
        }


        protected override void OnEnable()
        {
        }

        //Called whenever the condition gets disabled.
        protected override void OnDisable()
        {
        }

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            foreach (var player in playerGroup)
            {
                var curPos = owner.GetWorldPos().Value;
                return Vector3.Distance(curPos, player.GetWorldPos().Value) <= MaxDistance;
            }

            return false;
        }

        protected override string info
        {
            get { return "Distance" + "<=" + MaxDistance + " to player"; }
        }
    }
}