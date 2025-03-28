//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoCollisionMsgComponent
{
    
    public static void AddCollisionMsgComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.CollisionMsgComponent);
    }
    
    public static void AddCollisionMsgComponent(this ECSEntity ecsEntity,GXGame.CollisionMsg param)
    {
        var p  =  (GXGame.CollisionMsgComponent)ecsEntity.AddComponent(Components.CollisionMsgComponent);
        p.Value = param;
    }
          
    public static GXGame.CollisionMsgComponent GetCollisionMsgComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.CollisionMsgComponent)ecsEntity.GetComponent(Components.CollisionMsgComponent);
    }
     
    public static ECSEntity SetCollisionMsgComponent(this ECSEntity ecsEntity,GXGame.CollisionMsg param)
    {
        var p = (GXGame.CollisionMsgComponent)ecsEntity.GetComponent(Components.CollisionMsgComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.CollisionMsgComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetCollisionMsgComponent(this ECSEntity ecsEntity,GXGame.CollisionMsg param)
    {
        var p = (GXGame.CollisionMsgComponent)ecsEntity.GetComponent(Components.CollisionMsgComponent);
        if(p==null)
        {
           p = (GXGame.CollisionMsgComponent)(ecsEntity.AddComponent(Components.CollisionMsgComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.CollisionMsgComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}