using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class OprationSystem
    {
        private void UpdateThrow(ECSEntity ecsEntity)
        {
            var operation = ecsEntity.GetOperationComponent().Value;
            if (operation.Throw && operation.OperationTarget != null && operation.OperationTarget.IsAction)
            {
                var ecsCaughtComp = operation.OperationTarget.GetCaughtComponent();
                if (ecsCaughtComp == null)
                    return;
                //删除强制移动
                ecsCaughtComp.Value.RemoveComponent(Components.BeingCaughtComponent);
                ecsCaughtComp.Value.RemoveComponent(Components.ForceWorldPos);
                ecsCaughtComp.Value.RemoveComponent(Components.ForceWorldRotate);
                EasyComponent.AddBoxCollisionComponent(ecsCaughtComp.Value);
                ecsCaughtComp.Value.SetMoveDirection(operation.OperationTarget.GetWorldRotate().Value * Vector3.forward);
                ecsCaughtComp.Value.SetMoveSpeed(10);
                operation.OperationTarget.RemoveComponent(Components.CaughtComponent);
            }
        }
    }
}