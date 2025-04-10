using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("摄像机行为")]
    [Description("摄像机追踪")]
    public class CameraAction : ActionTask
    {
        private ECSEntity owner;
        private World world;
        private Group playerGroup;
        private Vector3 player2Camera;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            Matcher matcher = Matcher.SetAll(Components.Player);
            playerGroup = world.GetGroup(matcher);
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            foreach (var player in playerGroup)
            {
                Vector3 pos = owner.GetWorldPos().Value;
                Quaternion rot = owner.GetWorldRotate().Value;
                var dir = owner.GetMoveDirection().Value;
                Vector3 playerPos = player.GetWorldPos().Value;
                // Vector3 initPos = pos;
                if (player2Camera != Vector3.zero)
                {
                    pos = player2Camera.normalized * 5 + playerPos;
                }
                RotateAround(ref pos, playerPos, Vector3.up, dir.x);
                RotateAround(ref pos, playerPos, rot * Vector3.right, dir.y);
                owner.SetWorldPos(pos);
                player2Camera = pos - playerPos;
                rot = Quaternion.LookRotation(playerPos - pos);
                owner.SetWorldRotate(rot);
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

        public static void RotateAround(ref Vector3 pos, Vector3 point, Vector3 axis, float angle)
        {
            Vector3 offset = pos - point;

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);

            offset = rotation * offset;

            pos = point + offset;
        }
    }
}