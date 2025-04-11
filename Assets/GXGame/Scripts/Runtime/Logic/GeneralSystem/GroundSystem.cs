using GameFrame;

namespace GXGame
{
    public class GroundSystem : IInitializeSystem<World>, IFixedUpdateSystem
    {
        private Group group;
        private World world;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAny(Components.BoxColliderComponent, Components.CapsuleColliderComponent);
            this.world = world;
            group = world.GetGroup(matcher);
        }

        public void FixedUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group)
            {
                if (entity.HasComponent(Components.CapsuleColliderComponent))
                    CheckGroundedCapsule(entity);
            }
        }

        private void CheckGroundedCapsule(ECSEntity entity)
        {
            // var pos = entity.GetWorldPos().Value;
            // var rot = entity.GetWorldRotate().Value;
            // var ii = entity.get
            // bool onGround = CollisionDetection.CapsuleCastNonAlloc(pos, rot, Vector3.down, collisionMsg.groundDist, out RaycastHit groundHit,
            //     collisionMsg.MaskLayer, collisionMsg.skinWidth);
            // float angle = Vector3.Angle(groundHit.normal, Vector3.up);
            // return (onGround, angle, groundHit);
        }

        public void Dispose()
        {
            // TODO 在此释放托管资源
        }
    }
}