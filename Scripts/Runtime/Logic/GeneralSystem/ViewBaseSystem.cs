using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class ViewBaseSystem : UpdateReactiveSystem
    {
        private Camera camera;

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddRemoveUpdate, Components.ViewType);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            ViewTypeControl(entities);
        }

        public override void Dispose()
        {
        }

        private void ViewTypeControl(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent(Components.ViewCull))
                {
                    bool isInView = IsObjectInView(entity);
                    var view = entity.GetView();
                    if (isInView && view == null)
                    {
                        LoadAsset(entity);
                    }
                    else if (view != null && !isInView)
                    {
                        entity.RemoveComponent(Components.View);
                    }
                }
                else
                {
                    var view = entity.GetView();
                    if (view == null)
                    {
                        LoadAsset(entity);
                    }
                }
            }
        }

        private void LoadAsset(ECSEntity ecsentity)
        {
            Type type = ecsentity.GetViewType().Value;
            var objectView = View.Create(type);
            objectView.Link(ecsentity);
            ecsentity.AddView(objectView);
        }

        private bool IsObjectInView(ECSEntity ecsentity)
        {
            var pos = ecsentity.GetWorldPos();
            camera ??= Camera.main;
            Vector3 viewPos = camera.WorldToViewportPoint(pos.Value);
            bool isInView = viewPos.x > 0 && viewPos.x < 1 &&
                            viewPos.y > 0 && viewPos.y < 1 &&
                            viewPos.z > camera.nearClipPlane && viewPos.z < camera.farClipPlane;
            return isInView;
        }
    }
}