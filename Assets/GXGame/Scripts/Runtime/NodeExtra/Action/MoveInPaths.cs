using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("通用组件")]
    [Description("按照路径移动,达到指定路径的终点就停止.并且返回true")]
    public class MoveInPaths : ActionTask
    {
        private ECSEntity owner;
        private World world;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }

        protected override void OnExecute()
        {
        }
        
        protected override void OnUpdate()
        {
            var ownerWorldPos = owner.GetWorldPos().Value;
            var pathData = owner.GetFindPathComponent().Value;
            var gridData = owner.GetGridDataComponent().Value;
            var moveSpeed = owner.GetMoveSpeed().Value;
            if (pathData.Path != null && pathData.Path.Count != 0)
            {
                var nextPos = pathData.Path[Mathf.Min(pathData.NextIndex, pathData.Path.Count)];
                var nextPosWorld = gridData.CellToWolrd(new Vector3Int(nextPos.x, 0, nextPos.y));
                var dir = (nextPosWorld - ownerWorldPos);
                float distance = dir.magnitude;
                dir = dir.normalized;
                var faceDir = dir;
                if (distance <= moveSpeed * world.DeltaTime)
                {
                    pathData.NextIndex++;
                    if (pathData.NextIndex >= pathData.Path.Count)
                    {
                        pathData.Path.Clear();
                        pathData.NextIndex = 0;
                        EndAction(true);
                        dir = Vector3.zero;
                    }
                    owner.SetFindPathComponent(pathData);
                }
                owner.SetMoveDirection(dir);
                owner.SetFaceDirection(faceDir);
            }
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