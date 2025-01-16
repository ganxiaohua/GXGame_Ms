//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using GXGame;

namespace Eden.Gameplay.Runtime
{
    public class EventSend : Singleton<EventSend>
    {
       
        public void FireTestEvent1(System.String key)
        {
            var allEntity = EventData.Instance.GetEntity(typeof(ITestEvent1));
            if (allEntity == null)
            {
                return;
            }
            foreach (var entity in allEntity)
            {
                if (entity.State == IEntity.EntityState.IsRunning)
                {
                    ((ITestEvent1) entity).Test(key);
                }
            }
        }
    }
}