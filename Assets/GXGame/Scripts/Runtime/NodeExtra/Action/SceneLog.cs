using System.Diagnostics;
using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using Debugger = GameFrame.Debugger;

namespace GXGame {

	[Category("通用组件")]
	[Description("Log")]
	public partial class SceneLog : ActionTask {

		private ECSEntity owner;
		private World world;
		public float secondsToRun = 1f;
		[RequiredField]
		public BBParameter<string> log = "Hello World";
		protected override string OnInit() {
			owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
			world = ((World) owner.Parent);
			return null;
		}
		protected override void OnExecute() {
			Debugger.Log(log);
			OpenGuid(true);
		}
		protected override void OnUpdate() {
			if ( elapsedTime >= secondsToRun ) {
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			OpenGuid(false);
		}
		
		[Conditional("UNITY_EDITOR")]
		private  void OpenGuid(bool open)
		{
			if(open)
				MonoManager.current.onGUI += OnGUI;
			else
			{
				MonoManager.current.onGUI -= OnGUI;
			}
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
		
		protected override string info {
			get { return log.value; }
		}
	}
}