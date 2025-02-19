//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoGroundMsgComponent
{
    
    public static void AddGroundMsgComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.GroundMsgComponent);
    }
    
    public static void AddGroundMsgComponent(this ECSEntity ecsEntity,GXGame.GroudMsg param)
    {
        var p  =  (GXGame.GroundMsgComponent)ecsEntity.AddComponent(Components.GroundMsgComponent);
        p.Value = param;
    }
          
    public static GXGame.GroundMsgComponent GetGroundMsgComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.GroundMsgComponent)ecsEntity.GetComponent(Components.GroundMsgComponent);
    }
     
    public static ECSEntity SetGroundMsgComponent(this ECSEntity ecsEntity,GXGame.GroudMsg param)
    {
        var p = (GXGame.GroundMsgComponent)ecsEntity.GetComponent(Components.GroundMsgComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.GroundMsgComponent, ecsEntity);
        
        return ecsEntity;
    }
         
}