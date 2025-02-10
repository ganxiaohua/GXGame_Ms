using GameFrame;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using UnityEngine;


namespace GXGame
{
    [Category("摄像机行为")]
    [Description("摄像机追踪")]
    public class CameraFollow : ActionTask
    {
        private ECSEntity owner;
        private World world;

        private Group playerGroup;

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
                Vector3 playerWolrdPos = player.GetWorldPos().Value;
                Vector3 cameraWolrdPos = playerWolrdPos + new Vector3(0, 3, -3);
                owner.SetWorldPos(cameraWolrdPos);
                var angle = Vector3.Angle((playerWolrdPos - cameraWolrdPos), Vector3.forward);
                owner.SetWorldRotate(Quaternion.Euler(new Vector3(angle, 0, 0)));
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