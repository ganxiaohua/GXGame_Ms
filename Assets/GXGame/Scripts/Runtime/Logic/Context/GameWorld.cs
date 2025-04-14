using GameFrame;
using RVO;
using UnityEngine;

namespace GXGame.Logic
{
    public class GameWorld : World
    {
        private int otherCount = 30;
        private int monsterCount = 0;
        private GridData map;
        private GridData chickenHome;
        private ECSEntity player;

        public override void OnInitialize()
        {
            Random.InitState(0);
            Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 0.5f, 10, new Vector2(0.0f, 0.0f));
            base.OnInitialize();
            map = GameObject.Find("Map").GetComponent<GridData>();
            chickenHome = GameObject.Find("ChickenHome").GetComponent<GridData>();
            EstimateChildsCount(monsterCount + otherCount);
            this.AddSystem<ViewBaseSystem>();
            this.AddSystem<ViewUpdateSystem>();
            this.AddSystem<GroundSystem>(); //1
            this.AddSystem<GravitySystem>(); //2
            this.AddSystem<CollisionWorldPosDirSystem>(); //3
            // this.AddSystem<CollisionBehaviorSystem>();
            this.AddSystem<BeingCaughtSystem>();
            this.AddSystem<CommonWorldPosChangeSystem>();
            this.AddSystem<CommonWorldDirChangeBaseSystem>();
            this.AddSystem<RovPosSystem>();
            this.AddSystem<FindPathSystem>();
            this.AddSystem<FeedbackBoxSystem>();
            this.AddSystem<InputPcSystem>();
            this.AddSystem<OprationSystem>();
            //最后执行
            this.AddSystem<DestroyBaseSystem>();

            CreateCamera();
            CreatePlayer();
            CreatePublicEntity();
            // CreateMonster();
            CreateChicken();
            CreateChickenEgg();
            CreateBone();
        }

        private void CreateBone()
        {
            var egg = AddChild();
            egg.Name = $"骨头";
            egg.AddGridDataComponent(chickenHome);
            egg.AddViewType(typeof(GoBaseView));
            egg.AddAssetPath("Product/Bone/Bone");
            egg.AddDirectionSpeed(180);
            egg.AddMoveDirection(new Vector3(0, 0, 0));
            egg.AddFaceDirection();
            egg.AddMoveSpeed(3);
            egg.AddWorldPos(new Vector3(0, 0, -2));
            egg.AddWorldRotate(Quaternion.identity);
            egg.AddLocalScale(new Vector3(1.0f, 0.2f, 0.2f));
            egg.AddYAxisASpeed(0);
            egg.AddGravityDirComponent();
            egg.AddBoxColliderComponent(BoxColliderData.Create(egg, LayerMask.NameToLayer($"Interaction")));
            egg.AddUnitTypeComponent(UnitTypeEnum.Bone);
        }

        private void CreatePublicEntity()
        {
            var camera = AddChild();
            camera.Name = $"公共实体";
            camera.AddOperationComponent(new Operation() {OperationTarget = player});
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

        private void CreateChicken()
        {
            for (int i = 0; i < 4; i++)
            {
                var pos = chickenHome.CellToWolrd(new Vector2Int(i, 1));
                int id = Simulator.Instance.addAgent(new Vector2(pos.x, pos.z));
                Simulator.Instance.setAgentRadius(id, 0.8f);
                var monster = AddChild();
                monster.Name = $"鸡{i}";
                monster.AddViewType(typeof(GoBaseView));
                monster.AddAssetPath("Animal/Chicken/Chicken");
                monster.AddRovAgent(id);
                monster.AddWorldPos(pos);
                monster.AddWorldRotate(Quaternion.identity);
                monster.AddMoveSpeed(1.0f);
                monster.AddDirectionSpeed(180);
                monster.AddFaceDirection();
                monster.AddLocalScale(Vector3.one * 0.8f);
                monster.AddMoveDirection();
                monster.AddFindPathComponent(new FindPathData());
                monster.AddGridDataComponent(chickenHome);
                monster.AddBehaviorTreeComponent("BTO/IivestockBto");
                monster.AddBoxColliderComponent(BoxColliderData.Create(monster, LayerMask.NameToLayer($"Interaction")));
            }
        }


        private void CreateChickenEgg()
        {
            for (int i = 0; i < 4; i++)
            {
                var pos = chickenHome.CellToWolrd(new Vector2Int(i, 0));
                var egg = AddChild();
                egg.Name = $"鸡蛋{i}";
                egg.AddGridDataComponent(chickenHome);
                egg.AddViewType(typeof(GoBaseView));
                egg.AddAssetPath("Product/Egg/Egg");
                egg.AddWorldPos(pos);
                egg.AddWorldRotate(Quaternion.identity);
                egg.AddLocalScale(new Vector3(0.8f, 0.8f, 0.8f));
                egg.AddBoxColliderComponent(BoxColliderData.Create(egg, LayerMask.NameToLayer($"Interaction")));
                egg.AddUnitTypeComponent(UnitTypeEnum.AnimalProducts);
            }
        }

        private void CreatePlayer()
        {
            player = AddChild();
            player.Name = $"主角";
            player.AddViewType(typeof(AnimationView));
            player.AddAssetPath("Player/Prefabs/Player");
            player.AddWorldPos(new Vector3(0, 0, 0));
            player.AddLocalScale(Vector3.one);
            player.AddMoveDirection();
            player.AddWorldRotate(Quaternion.identity);
            player.AddMoveSpeed(3.2f);
            player.AddFaceDirection();
            player.AddDirectionSpeed(360);
            player.AddYAxisASpeed(0);
            player.AddGravityComponent(12);
            player.AddGravityDirComponent(Vector3.zero);
            player.AddGroundCollisionComponent(new GroundCollision());
            player.AddPreviousGroundMsgComponent(new PreviousGroundMsg());
            player.AddCollisionMsgComponent(new CollisionMsg()
            {
                MaskLayer = ~(1 << LayerMask.NameToLayer("NoInteraction") | 1 << LayerMask.NameToLayer("Player"))
            });
            player.AddCapsuleColliderComponent(CapsuleColliderData.Create(player, LayerMask.NameToLayer($"Player")));
            player.AddCollisionGroundType(CollisionGroundType.Slide);
            player.AddFeedBackBoxComponent(new FeedBackBoxData()
            {
                Size = new Vector3(1, 1, 1),
                MaskLayer = 1 << LayerMask.NameToLayer("Interaction"),
            });
            player.AddCampComponent(GXGame.Camp.SELF);
            player.AddGXInput();
            player.AddPlayer();
            player.AddHP(10);
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
                monster.AddGridDataComponent(map);
                monster.AddBehaviorTreeComponent("BTO/Monster01Bto");
                monster.AddBoxColliderComponent(BoxColliderData.Create(monster, LayerMask.NameToLayer($"Monster")));
            }
        }
    }
}