using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GXGame
{
    [Category("怪物AI")]
    [Description("寻路追踪主角")]
    public class TracePlayer : ActionTask
    {
        private ECSEntity owner;
        private World world;
        private Group playerGroup;
        private Vector3 player2Camera;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            Matcher matcher = Matcher.SetAll(Components.Player);
            playerGroup = world.GetGroup(matcher);
            return null;
        }


        protected override void OnExecute()
        {
            foreach (var player in playerGroup)
            {
                var playerWorldPos = player.GetWorldPos().Value;
                var moveSpeed = player.GetMoveSpeed().Value;
                var ownerWorldPos = owner.GetWorldPos().Value;
                var targetPos = owner.GetPathFindingTargetPos().Value;
                var pathData = owner.GetFindPathComponent().Value;
                var gridData = owner.GetGridDataComponent().Value;
                if (pathData.Path != null && pathData.Path.Count != 0)
                {
                    var nextPos = pathData.Path[Mathf.Min(pathData.NextIndex, pathData.Path.Count)];
                    var nextPosWorld = gridData.CellToWolrd(new Vector3Int(nextPos.x, 0, nextPos.y));
                    var dir = (nextPosWorld - ownerWorldPos).normalized;
                    owner.SetMoveDirection(dir);
                    // owner.SetFaceDirection(dir);
                    if ( dir.magnitude <= moveSpeed * world.DeltaTime)
                    {
                        pathData.NextIndex++;
                        owner.SetFindPathComponent(pathData);
                    }
                }

                if (Vector3.Distance(playerWorldPos, targetPos) > 1)
                {
                    owner.SetPathFindingTargetPos(playerWorldPos);
                }
            }

            EndAction(true);
        }

        //Called once per frame while the action is active.
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

        public static void RotateAround(ref Vector3 pos, Vector3 point, Vector3 axis, float angle)
        {
            Vector3 offset = pos - point;

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);

            offset = rotation * offset;

            pos = point + offset;
        }
    }
}