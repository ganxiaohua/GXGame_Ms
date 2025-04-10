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
            foreach (var input in inputs)
            {
                var feedBackBoxValue = input.GetFeedBackBoxComponent().Value;
                if (feedBackBoxValue.FeedBackEntitys == null)
                    continue;
                var targetEntity = GetDisRecentEntity(input, feedBackBoxValue.FeedBackEntitys);
                if (operation.Interaction && targetEntity != null)
                {
                    targetEntity.AddDestroy();
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
    }
}