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
#if UNITY_EDITOR
            CollisionDetection.Init();
#endif
        }

        public override void Dispose()
        {
#if UNITY_EDITOR
            CollisionDetection.Clear();
#endif
            base.Dispose();
        }
    }
}