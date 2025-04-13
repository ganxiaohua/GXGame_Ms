//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoMoveDirection
{
    
    public static void AddMoveDirection(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.MoveDirection);
    }
    
    public static void AddMoveDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p  =  (GXGame.MoveDirection)ecsEntity.AddComponent(Components.MoveDirection);
        p.Value = param;
    }
          
    public static GXGame.MoveDirection GetMoveDirection(this ECSEntity ecsEntity)
    {
        return (GXGame.MoveDirection)ecsEntity.GetComponent(Components.MoveDirection);
    }
     
    public static ECSEntity SetMoveDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.MoveDirection)ecsEntity.GetComponent(Components.MoveDirection);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.MoveDirection, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetMoveDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.MoveDirection)ecsEntity.GetComponent(Components.MoveDirection);
        if(p==null)
        {
           p = (GXGame.MoveDirection)(ecsEntity.AddComponent(Components.MoveDirection));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.MoveDirection, ecsEntity);
        
        return ecsEntity;
    } 
         
}