using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class Go3DView : GameObjectView
    {
        private AnimatorView animator;

        private int walkId = Animator.StringToHash("Walk");
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

        public override void WolrdPosition(WorldPos worldPos)
        {
            base.WolrdPosition(worldPos);
            MoveAnimation();
        }

        private void MoveAnimation()
        {
            var dir = BindEntity.GetMoveDirection().Value;
            if (dir != Vector3.zero)
            {
                animator.Play(walkId);
            }
            else
            {
                animator.Play(idleId);
            }
        }
    }
}