using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame {

	[Category("怪物AI")]
	[Description("朝向目标")]
	public class TurnTarget : ActionTask {

		private ECSEntity owner;
		private World world;
		private Group playerGroup;
		private int tagertId;
		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
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
				var dir = (player.GetWorldPos().Value - owner.GetWorldPos().Value).normalized;
				owner.SetMoveDirection(Vector3.zero);
				owner.SetFaceDirection(dir);
				break;
			}
			EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}