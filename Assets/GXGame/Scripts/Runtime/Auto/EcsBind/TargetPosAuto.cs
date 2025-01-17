//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoTargetPos
{
    
    public static void AddTargetPos(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.TargetPos);
    }
    
    public static void AddTargetPos(this ECSEntity ecsEntity,UnityEngine.Vector2 param)
    {
        var p  =  (GXGame.TargetPos)ecsEntity.AddComponent(Components.TargetPos);
        p.Value = param;
    }
          
    public static GXGame.TargetPos GetTargetPos(this ECSEntity ecsEntity)
    {
        return (GXGame.TargetPos)ecsEntity.GetComponent(Components.TargetPos);
    }
     
    public static ECSEntity SetTargetPos(this ECSEntity ecsEntity,UnityEngine.Vector2 param)
    {
        var p = (GXGame.TargetPos)ecsEntity.GetComponent(Components.TargetPos);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.TargetPos, ecsEntity);
        
        return ecsEntity;
    }
         
}