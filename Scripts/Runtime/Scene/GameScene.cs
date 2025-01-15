using GameFrame;
using GXGame.Logic;

namespace GXGame
{
    public class GameScene : SceneBase
    {
        protected override string SingleSceneName => "Scene_Game";
        protected override void OnReady()
        {
            AddComponent<GameWorld>();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}