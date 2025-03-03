using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame {

	[Category("怪物AI")]
	[Description("随机获取可行走地图上的一点,获取结束之后返回true")]
	public class FindPatrolPos : ActionTask {
		private ECSEntity owner;
		private World world;
		protected override string OnInit() {
			owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
			world = ((World) owner.Parent);
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			
			var gridData = owner.GetGridDataComponent().Value;
			int index = UnityEngine.Random.Range(0, gridData.NoObstacleCells.Count);
			var pos = gridData.NoObstacleCells[index];
			var worldPos = gridData.CellToWolrd(new Vector3Int(pos.x,0,pos.y));
			owner.SetPathFindingTargetPos(worldPos);
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