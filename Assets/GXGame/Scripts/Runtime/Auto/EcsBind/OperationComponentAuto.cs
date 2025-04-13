//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoOperationComponent
{
    
    public static void AddOperationComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.OperationComponent);
    }
    
    public static void AddOperationComponent(this ECSEntity ecsEntity,GXGame.Operation param)
    {
        var p  =  (GXGame.OperationComponent)ecsEntity.AddComponent(Components.OperationComponent);
        p.Value = param;
    }
          
    public static GXGame.OperationComponent GetOperationComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.OperationComponent)ecsEntity.GetComponent(Components.OperationComponent);
    }
     
    public static ECSEntity SetOperationComponent(this ECSEntity ecsEntity,GXGame.Operation param)
    {
        var p = (GXGame.OperationComponent)ecsEntity.GetComponent(Components.OperationComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.OperationComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetOperationComponent(this ECSEntity ecsEntity,GXGame.Operation param)
    {
        var p = (GXGame.OperationComponent)ecsEntity.GetComponent(Components.OperationComponent);
        if(p==null)
        {
           p = (GXGame.OperationComponent)(ecsEntity.AddComponent(Components.OperationComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.OperationComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}