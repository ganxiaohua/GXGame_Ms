using System;
using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("AI")]
    [Description("寻找追踪目标路径")]
    public class FindTargetPathAction : ActionTask
    {
        private ECSEntity owner;
        private World world;
        private Group targetGroup;
        private Vector3Int? targetPos;

        public BBParameter<Int32> PlayerComponents;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            Matcher matcher = Matcher.SetAll(PlayerComponents.value);
            targetGroup = world.GetGroup(matcher);
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            foreach (var target in targetGroup)
            {
                var gridData = owner.GetGridDataComponent().Value;
                var targetWorldPos = target.GetWorldPos().Value;
                var worldCell = gridData.WorldToCell(targetWorldPos);
                if (targetPos == null || worldCell != targetPos.Value)
                {
                    owner.SetPathFindingTargetPos(targetWorldPos);
                    targetPos = worldCell;
                    break;
                }
            }

            EndAction(true);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
        }

        //Called when the task is paused.
        protected override void OnPause()
        {
        }
    }
}