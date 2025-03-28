//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoFindPathComponent
{
    
    public static void AddFindPathComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.FindPathComponent);
    }
    
    public static void AddFindPathComponent(this ECSEntity ecsEntity,GXGame.FindPathData param)
    {
        var p  =  (GXGame.FindPathComponent)ecsEntity.AddComponent(Components.FindPathComponent);
        p.Value = param;
    }
          
    public static GXGame.FindPathComponent GetFindPathComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.FindPathComponent)ecsEntity.GetComponent(Components.FindPathComponent);
    }
     
    public static ECSEntity SetFindPathComponent(this ECSEntity ecsEntity,GXGame.FindPathData param)
    {
        var p = (GXGame.FindPathComponent)ecsEntity.GetComponent(Components.FindPathComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.FindPathComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetFindPathComponent(this ECSEntity ecsEntity,GXGame.FindPathData param)
    {
        var p = (GXGame.FindPathComponent)ecsEntity.GetComponent(Components.FindPathComponent);
        if(p==null)
        {
           p = (GXGame.FindPathComponent)(ecsEntity.AddComponent(Components.FindPathComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.FindPathComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}