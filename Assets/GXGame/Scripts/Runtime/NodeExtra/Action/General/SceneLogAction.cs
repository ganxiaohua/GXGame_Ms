using System.Diagnostics;
using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using Debugger = GameFrame.Debugger;

namespace GXGame
{
    [Category("通用组件")]
    [Description("Log")]
    public partial class SceneLogAction : ActionTask
    {
        private ECSEntity owner;
        private World world;
        public float SecondsToRun = 1f;
        [RequiredField] public BBParameter<string> LOG = "Hello World";

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }

        protected override void OnExecute()
        {
            Debugger.Log(LOG);
#if UNITY_EDITOR
            OpenGuid(true);
#endif
            EndAction(true);
        }

        protected override void OnUpdate()
        {
            if (elapsedTime >= SecondsToRun)
            {
                EndAction(true);
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
#if UNITY_EDITOR
            OpenGuid(false);
#endif
        }

#if UNITY_EDITOR
        private void OpenGuid(bool open)
        {
            if (open)
                MonoManager.current.onGUI += OnGUI;
            else
            {
                MonoManager.current.onGUI -= OnGUI;
            }
        }
#endif

        //Called when the task is paused.
        protected override void OnPause()
        {
        }

        protected override string info
        {
            get { return LOG.value; }
        }
    }
}