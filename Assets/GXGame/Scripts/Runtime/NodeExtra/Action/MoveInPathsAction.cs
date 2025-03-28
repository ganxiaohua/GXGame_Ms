using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("通用组件")]
    [Description("按照路径移动,达到指定路径的终点就停止.并且返回true")]
    public class MoveInPathsAction : ActionTask
    {
        private ECSEntity owner;
        private World world;
        public bool isTowardPath;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }


        protected override void OnExecute()
        {
            bool action = false;
            var ownerWorldPos = owner.GetWorldPos().Value;
            if (!owner.HasComponent(Components.FindPathComponent))
            {
                EndAction(false);
                return;
            }

            var pathData = owner.GetFindPathComponent().Value;
            var gridData = owner.GetGridDataComponent().Value;

            var moveSpeed = owner.GetMoveSpeed().Value;
            if (pathData.Path != null && pathData.Path.Count != 0)
            {
                action = true;
                var nextPos = pathData.Path[Mathf.Min(pathData.NextIndex, pathData.Path.Count)];
                var nextPosWorld = gridData.CellToWolrd(nextPos);
                var dir = (nextPosWorld - ownerWorldPos);
                float distance = dir.magnitude;
                dir = dir.normalized;
                var faceDir = dir;
                if (distance <= Mathf.Min(moveSpeed * world.DeltaTime, 0.05f))
                {
                    pathData.NextIndex++;
                    if (pathData.NextIndex >= pathData.Path.Count)
                    {
                        pathData.Path.Clear();
                        pathData.NextIndex = 0;
                        dir = Vector3.zero;
                        action = false;
                    }

                    owner.SetFindPathComponent(pathData);
                }

                owner.SetMoveDirection(dir);
                if (isTowardPath)
                    owner.SetFaceDirection(faceDir);
            }

            EndAction(action);
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