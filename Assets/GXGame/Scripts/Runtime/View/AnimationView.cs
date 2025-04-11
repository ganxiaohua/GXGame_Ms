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
        private int Falling = Animator.StringToHash("Falling");

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
            var ground = BindEntity.GetPreviousGroundMsgComponent().Value;
            if (dir == null || ground == null)
                return;
            if (ground.PreviousParent == null)
            {
                animator.Play(Falling);
            }
            else
            {
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
}