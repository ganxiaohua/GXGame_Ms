using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GXGame
{
    public class CameraView : GameObjectView
    {
        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().Value).Forget();
            OnWait().Forget();
        }


        private async UniTaskVoid OnWait()
        {
            await WaitLoadOver();
            SetCamera();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void SetCamera()
        {
            var camera = GXGO.gameObject.GetComponent<Camera>();
            var uiCamera = GameObject.Find("Stage Camera").GetComponent<Camera>();
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var cameraData = camera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }
    }
}