﻿using GameFrame;
using UnityEngine;


namespace GXGame
{
    public class Go2DView : GameObjectView
    {
        private AnimatorView animator;
        private SpriteRendererView spriterenderer;

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().Value).Forget();
            spriterenderer = ReferencePool.Acquire<SpriteRendererView>();
            spriterenderer.Init(ecsEntity, this);
            animator = ReferencePool.Acquire<AnimatorView>();
            animator.Init(ecsEntity, this);
        }


        public override void WolrdPosition(WorldPos worldPos)
        {
            base.WolrdPosition(worldPos);
            MoveAnimation();
        }

        private void MoveAnimation()
        {
            var dir = BindEntity.GetFaceDirection().Value;
            var scale = BindEntity.GetLocalScale().Value;
            if (dir != Vector3.zero)
            {
                animator.SetBool("Atk", false);
                animator.SetBool("Stop", false);
                animator.SetInteger("State", 1);
                GXGO.scale = dir.x switch
                {
                    > 0 => new Vector3(scale.x, scale.y, scale.z),
                    < 0 => new Vector3(-scale.x, scale.y, scale.z),
                    _ => GXGO.scale
                };
                if (dir.y < 0)
                    animator.SetInteger("Direction", 1);
                else if (dir.y > 0)
                    animator.SetInteger("Direction", 3);

                if (dir.x > 0 || dir.x < 0)
                {
                    animator.SetInteger("Direction", 2);
                }
            }
            else
            {
                animator.SetBool("Stop", true);
            }
        }


        public override void Dispose()
        {
            ReferencePool.Release(spriterenderer);
            ReferencePool.Release(animator);
            animator = null;
            spriterenderer = null;
            base.Dispose();
        }
    }
}