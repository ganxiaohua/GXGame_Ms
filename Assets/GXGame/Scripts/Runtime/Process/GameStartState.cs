using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class GameStartState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            QualitySettings.vSyncCount = 0;
            Time.fixedDeltaTime = 1/50.0f;
            Application.targetFrameRate = 10;
            SceneFactory.ChangePlayerScene<GameScene>(this);
        }
    }
}