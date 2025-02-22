using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class AnimationView : GameObjectView
    {
        private AnimatorView animator;

        private int walkId = Animator.StringToHash("Walk");
        private int RunId = Animator.StringToHash("Run");
        private int idleId = Animator.StringToHash("Idle");

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().Value).Forget();
            animator = ReferencePool.Acquire<AnimatorView>();
            animator.Init(BindEntity, this);
        }

        public override void Dispose()
        {
            ReferencePool.Release(animator);
            base.Dispose();
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            MoveAnimation();
        }

        private void MoveAnimation()
        {
            var dir = BindEntity.GetMoveDirection();
            if (dir == null)
                return;
            if (dir.Value != Vector3.zero)
            {
                animator.Play(RunId);
            }
            else
            {
                animator.Play(idleId);
            }
        }
    }
}