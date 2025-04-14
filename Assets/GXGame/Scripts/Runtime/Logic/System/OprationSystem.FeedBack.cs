using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class OprationSystem
    {
        private void UpdateFeedBack(ECSEntity ecsEntity)
        {
            var operation = ecsEntity.GetOperationComponent().Value;
            if (operation.Interaction && operation.OperationTarget != null && operation.OperationTarget.IsAction)
            {
                var feedBackBoxValue = operation.OperationTarget.GetFeedBackBoxComponent().Value;
                if (feedBackBoxValue.FeedBackEntitys == null)
                    return;
                var targetEntity = GetDisRecentEntity(operation.OperationTarget, feedBackBoxValue.FeedBackEntitys);
                if (targetEntity != null)
                {
                    FeedBackLogic(operation.OperationTarget, targetEntity);
                }
            }
        }

        private ECSEntity GetDisRecentEntity(ECSEntity ecsEntity, List<ECSEntity> feedBackEntitys)
        {
            ECSEntity targetEntity = null;
            float dis = float.MaxValue;
            var playerPos = ecsEntity.GetWorldPos().Value;
            foreach (var feedBackEntity in feedBackEntitys)
            {
                if (!feedBackEntity.IsAction)
                    continue;
                var feedBackPos = feedBackEntity.GetWorldPos().Value;
                var tempdis = Vector3.Distance(playerPos, feedBackPos);
                if (tempdis < dis)
                {
                    dis = tempdis;
                    targetEntity = feedBackEntity;
                }
            }

            return targetEntity;
        }

        /// <summary>
        /// 被操作逻辑
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="byOperator"></param>
        private void FeedBackLogic(ECSEntity oper, ECSEntity byOperator)
        {
            var type = byOperator.GetUnitTypeComponent().Value;
            if (type == UnitTypeEnum.Bone && byOperator.GetBeingCaughtComponent() == null)
            {
                oper.AddCaughtComponent(byOperator);
                byOperator.AddBeingCaughtComponent(new BeingCaughtData() {Holder = oper, Offset = new Vector3(0, 1, 0f)});
            }
            else if (type == UnitTypeEnum.AnimalProducts)
            {
                byOperator.AddDestroy();
            }
        }
    }
}