using System.Collections.Generic;
using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("AI")]
    [Description("随机获取可行走地图上的一点,如果本身和获取点的距离相距过近重新获取,一直返回true")]
    public class FindPatrolPosAction : ActionTask
    {
        private ECSEntity owner;
        private World world;
        private List<Vector2Int> patrolPoint;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            patrolPoint = new List<Vector2Int>();
            patrolPoint.Add(new Vector2Int(16, 2));
            patrolPoint.Add(new Vector2Int(16, 7));
            patrolPoint.Add(new Vector2Int(3, 3));
            patrolPoint.Add(new Vector2Int(5, 15));
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            var gridData = owner.GetGridDataComponent().Value;
            var pathData = owner.GetFindPathComponent().Value;
            var ownerPos = owner.GetWorldPos().Value;
            var moveSpeed = owner.GetMoveSpeed().Value;

            if (pathData.Path != null && pathData.Path.Count != 0)
            {
                var destination = pathData.Path[^1];
                var nextPosWorld = gridData.CellToWolrd(destination);
                var dir = nextPosWorld - ownerPos;
                float distance = dir.magnitude;
                if (distance > Mathf.Min(moveSpeed * world.DeltaTime, 0.05f))
                {
                    EndAction(true);
                }
            }
            else if (pathData.Path != null && pathData.Path.Count == 0 && !pathData.IsFindPath)
            {
                int index = Random.Range(0, patrolPoint.Count);
                var pos = patrolPoint[index];
                var worldPos = gridData.CellToWolrd(pos);
                owner.SetPathFindingTargetPos(worldPos);
            }

            EndAction(true);
        }

        protected override void OnUpdate()
        {
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
        }

        //Called when the task is paused.
        protected override void OnPause()
        {
        }
    }
}