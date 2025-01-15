using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class AnimatorView : BaseView
    {
        private Animator mAnimator;

        private int curId;
        
        public override void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            base.Init(ecsEntity, gameObjectView);
            curId = -1;
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
        
        
        public void Play(int id)
        {
            if (mAnimator == null)
                return;
            if(curId == id)
                return;
            curId = id;
            mAnimator.CrossFadeInFixedTime(id,0.25f);
        }
    }
}