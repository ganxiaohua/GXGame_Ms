﻿using GameFrame;
using RVO;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int otherCount = 30;
        private int monsterCount = 0;
        private GridData gridData;

        public override void OnInitialize()
        {
            Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, 10, new Vector2(0.0f, 0.0f));
            base.OnInitialize();
            gridData = GameObject.Find("Map").GetComponent<GridData>();
            EstimateChildsCount(monsterCount + otherCount);
            this.AddSystem<ViewBaseSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<ControlSystem>();
            this.AddSystem<CollisionBehaviorSystem>();
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<RovPosSystem>();
            this.AddSystem<WorldDirChangeBaseSystem>();
            this.AddSystem<FindPathSystem>(gridData);
            this.AddSystem<InputSystem>();
            //最后执行
            this.AddSystem<DestroyBaseSystem>();
            CreateCamera();
            CreatePlayer();
            // CreatePotato();
            CreateMonster();
            // foreach (var cells in gridData.ObstacleCells)
            // {
            //     Simulator.Instance.addObstacle()
            // }
        }

        private void CreateCamera()
        {
            var camera = AddChild();
            camera.Name = $"摄像机";
            camera.AddCameraComponent();
            camera.AddViewType(typeof(CameraView));
            camera.AddAssetPath("Camera/Prefabs/Camera");
            camera.AddWorldPos(new Vector3(0, 3, -4));
            camera.AddWorldRotate(Quaternion.identity);
            camera.AddLocalScale(Vector3.one);
            camera.AddBehaviorTreeComponent("BTO/CameraBto");
            camera.AddMoveDirection(Vector3.zero);
        }

        private void CreatePotato()
        {
            for (int i = 0; i < 30; i++)
            {
                var Potato = AddChild();
                Potato.Name = $"土豆{i}";
                Potato.AddViewType(typeof(GoBaseView));
                Potato.AddAssetPath("Crop/Potato/Potato_0");
                Potato.AddWorldPos(new Vector3(i % 5, 0, -5 + i / 5));
                Potato.AddWorldRotate(Quaternion.identity);
                Potato.AddLocalScale(new Vector3(0.5f, 0.5f, 0.5f));
                Potato.AddBoxCollider(BoxCollider.Create(Potato, LayerMask.NameToLayer($"Crop")));
            }
        }


        private void CreatePlayer()
        {
            var palyer = AddChild();
            palyer.Name = $"主角";
            palyer.AddViewType(typeof(AnimationView));
            palyer.AddAssetPath("Player/Prefabs/Player");
            palyer.AddWorldPos(new Vector3(0, 0, 0));
            palyer.AddLocalScale(Vector3.one);
            palyer.AddMoveDirection();
            palyer.AddWorldRotate(Quaternion.identity);
            palyer.AddMoveSpeed(3.2f);
            palyer.AddFaceDirection();
            palyer.AddDirectionSpeed(360);
            palyer.AddYAxisASpeed(5);
            palyer.AddYAxisAcceleration(false);
            palyer.AddGroundMsgComponent(new GroudMsg());
            palyer.AddCollisionMsgComponent(new CollisionMsg() {MaskLayer = ~(1 << LayerMask.NameToLayer("Crop"))});
            palyer.AddCapsuleCollider(CapsuleCollider.Create(palyer, LayerMask.NameToLayer($"Object")));
            palyer.AddCollisionGroundType(CollisionGroundType.Slide);
            palyer.AddCampComponent(GXGame.Camp.SELF);
            palyer.AddGXInput();
            palyer.AddPlayer();
            palyer.AddHP(10);
        }

        private void CreateMonster()
        {
            for (int i = 0; i < 1; i++)
            {
                Vector3 initPos = new Vector3(i, 0, -5);
                int id = Simulator.Instance.addAgent(new Vector2(initPos.x, initPos.z));
                Simulator.Instance.setAgentMaxSpeed(id, 1.0f);
                Simulator.Instance.setAgentRadius(id, 0.5f);
                var monster = AddChild();
                monster.Name = $"怪兽";
                monster.AddViewType(typeof(GoBaseView));
                monster.AddAssetPath("Monster/Monster01/Monster01");
                monster.AddRovAgent(id);
                monster.AddWorldPos(initPos);
                monster.AddWorldRotate(Quaternion.identity);
                monster.AddMoveSpeed(1.0f);
                monster.AddDirectionSpeed(180);
                monster.AddFaceDirection();
                monster.AddLocalScale(Vector3.one);
                monster.AddPathFindingTargetPos();
                monster.AddMoveDirection();
                monster.AddFindPathComponent(new FindPathData());
                monster.AddGridDataComponent(gridData);
                monster.AddBehaviorTreeComponent("BTO/Monster01Bto");
            }
        }
    }
}