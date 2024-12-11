﻿using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class AnimatorView : BaseView
    {
        private Animator mAnimator;

        public override void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            base.Init(ecsEntity, gameObjectView);
        }

        protected override async UniTask WaitLoadOver()
        {
            await base.WaitLoadOver();
            mAnimator = GameObjectView.GXGO.gameObject.GetComponent<Animator>();
            mAnimator.enabled = true;
        }


        public override void Dispose()
        {
            if (mAnimator != null)
                mAnimator.enabled = false;
            mAnimator = null;
            base.Dispose();
        }

        public void Play(string animationName)
        {
            if (mAnimator == null)
                return;
            mAnimator.Play(animationName);
        }

        public void SetBool(string name, bool b)
        {
            if (mAnimator == null)
                return;
            mAnimator.SetBool(name, b);
        }

        public void SetInteger(string name, int b)
        {
            if (mAnimator == null)
                return;
            mAnimator.SetInteger(name, b);
        }
    }
}