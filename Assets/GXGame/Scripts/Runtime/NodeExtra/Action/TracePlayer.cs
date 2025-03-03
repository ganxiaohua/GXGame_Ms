using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("怪物AI")]
    [Description("追踪主角")]
    public class TracePlayer : ActionTask
    {
        private ECSEntity owner;
        private World world;
        private Group playerGroup;
        private Vector3 player2Camera;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            Matcher matcher = Matcher.SetAll(Components.Player);
            playerGroup = world.GetGroup(matcher);
            return null;
        }


        protected override void OnExecute()
        {
            foreach (var player in playerGroup)
            {
                var playerWorldPos = player.GetWorldPos().Value;
                var targetPos = owner.GetPathFindingTargetPos().Value;
                if (Vector3.Distance(playerWorldPos, targetPos) > 1)
                {
                    owner.SetPathFindingTargetPos(playerWorldPos);
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