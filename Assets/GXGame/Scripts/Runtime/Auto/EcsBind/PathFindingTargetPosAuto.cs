//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoPathFindingTargetPos
{
    
    public static void AddPathFindingTargetPos(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.PathFindingTargetPos);
    }
    
    public static void AddPathFindingTargetPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p  =  (GXGame.PathFindingTargetPos)ecsEntity.AddComponent(Components.PathFindingTargetPos);
        p.Value = param;
    }
          
    public static GXGame.PathFindingTargetPos GetPathFindingTargetPos(this ECSEntity ecsEntity)
    {
        return (GXGame.PathFindingTargetPos)ecsEntity.GetComponent(Components.PathFindingTargetPos);
    }
     
    public static ECSEntity SetPathFindingTargetPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.PathFindingTargetPos)ecsEntity.GetComponent(Components.PathFindingTargetPos);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.PathFindingTargetPos, ecsEntity);
        
        return ecsEntity;
    }
         
}