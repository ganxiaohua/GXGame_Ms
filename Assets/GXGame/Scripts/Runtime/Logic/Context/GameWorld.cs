﻿using GameFrame;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int otherCount = 2;
        private int monsterCount = 0;

        public override void OnInitialize()
        {
            base.OnInitialize();
            EstimateChildsCount(monsterCount + otherCount);
            this.AddSystem<ViewBaseSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<ControlSystem>();
            this.AddSystem<CollisionBehaviorSystem>();
            this.AddSystem<WorldPosChangeSystem>();
            this.AddSystem<WorldDirChangeBaseSystem>();
            this.AddSystem<CountDowntSystem>();
            this.AddSystem<InputSystem>();
            //最后执行
            this.AddSystem<DestroyBaseSystem>();
            CreateCamera();
            CreatePlayer();
        }

        private void CreateCamera()
        {
            var camera = AddChild();
            camera.Name = $"摄像机";
            camera.AddViewType(typeof(CameraView));
            camera.AddAssetPath("Camera/Prefabs/Camera");
            camera.AddWorldPos(Vector3.zero);
            camera.AddWorldRotate(Quaternion.identity);
            camera.AddLocalScale(Vector3.one);
            camera.AddBehaviorTreeComponent("BTO/CameraBto");
        }

        
        private void CreatePlayer()
        {
            var palyer = AddChild();
            palyer.Name = $"主角";
            palyer.AddViewType(typeof(Go3DView));
            palyer.AddAssetPath("Player/Prefabs/Player");
            palyer.AddWorldPos(new Vector3(0,1,0));
            palyer.AddLocalScale(Vector3.one);
            palyer.AddMoveDirection();
            palyer.AddWorldRotate(Quaternion.identity);
            palyer.AddMoveSpeed(3.2f);
            palyer.AddFaceDirection();
            palyer.AddDirectionSpeed(360);
            palyer.AddGravity(12f);
            palyer.AddYAxisASpeed(5);
            palyer.AddYAxisAcceleration(false);
            palyer.AddCapsuleCollider(CapsuleCollider.Create(palyer, LayerMask.NameToLayer($"Object")));
            palyer.AddCollisionGroundType(CollisionGroundType.Slide);
            palyer.AddCampComponent(GXGame.Camp.SELF);
            palyer.AddGXInput();
            palyer.AddPlayer();
            palyer.AddHP(10);

            // for (int i = 0; i < monsterCount; i++)
            // {
            //     var monster = AddChild();
            //     monster.Name = "骷髅";
            //     monster.AddViewType(typeof(Go2DView));
            //     monster.AddAssetPath("Monster_001/Prefab/Monster_001");
            //     monster.AddWorldPos(new Vector3(-0.5f + i % 50, 3.0f + i / 50, 0));
            //     monster.AddLocalScale(Vector2.one);
            //     monster.AddMoveDirection();
            //     monster.AddMoveSpeed(0.5f);
            //     monster.AddFaceDirection();
            //     // monster.AddCollisionBox(CollisionBox.Create(monster, LayerMask.NameToLayer($"Object")));
            //     // monster.AddCollisionGroundType(CollisionGroundType.Slide);
            //     monster.AddCampComponent(GXGame.Camp.ENEMY);
            //     monster.AddMonster();
            //     monster.AddHP(10);
            //     monster.AddGXInput();
            //     // monster.AddBehaviorTreeComponent("MonsterBTO");
            // }
            //
            // var monster1 = AddChild();
            // monster1.Name = "史莱姆";
            // monster1.AddViewType(typeof(Go2DView));
            // monster1.AddAssetPath("Monster_002/Prefab/Monster_002");
            // monster1.AddWorldPos(new Vector3(5.5f, 1, -1));
            // monster1.AddLocalScale(Vector2.one);
            // monster1.AddMoveDirection();
            // monster1.AddMoveSpeed(0.5f);
            // monster1.AddFaceDirection();
            // monster1.AddCollisionBox(CollisionBox.Create(monster1, LayerMask.NameToLayer($"Object")));
            // monster1.AddCollisionGroundType(CollisionGroundType.Slide);
            // monster1.AddMonster();
            // monster1.AddCampComponent(GXGame.Camp.ENEMY);
            // monster1.AddHP(10);
            // monster1.AddBehaviorTreeComponent("MonsterBTO");
            //
            // for (int i = 0; i < 2; i++)
            // {
            //     monster1 = AddChild();
            //     monster1.Name = "骷髅";
            //     monster1.AddViewType(typeof(Go2DView));
            //     monster1.AddAssetPath("Monster_001/Prefab/Monster_001");
            //     monster1.AddWorldPos(new Vector3(-1f, i, -1));
            //     monster1.AddLocalScale(Vector2.one);
            //     monster1.AddMoveDirection();
            //     monster1.AddMoveSpeed(1.0f);
            //     monster1.AddFaceDirection();
            //     monster1.AddCollisionBox(CollisionBox.Create(monster1, LayerMask.NameToLayer($"Object")));
            //     monster1.AddCollisionGroundType(CollisionGroundType.Slide);
            //     monster1.AddMonster();
            //     monster1.AddCampComponent(GXGame.Camp.ENEMY);
            //     monster1.AddHP(10);
            //     monster1.AddBehaviorTreeComponent("MonsterBTO");
            // }
        }
    }
}