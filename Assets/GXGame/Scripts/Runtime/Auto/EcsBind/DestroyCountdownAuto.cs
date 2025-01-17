//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoDestroyCountdown
{
    
    public static void AddDestroyCountdown(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.DestroyCountdown);
    }
    
    public static void AddDestroyCountdown(this ECSEntity ecsEntity,System.Single param)
    {
        var p  =  (GXGame.DestroyCountdown)ecsEntity.AddComponent(Components.DestroyCountdown);
        p.Value = param;
    }
          
    public static GXGame.DestroyCountdown GetDestroyCountdown(this ECSEntity ecsEntity)
    {
        return (GXGame.DestroyCountdown)ecsEntity.GetComponent(Components.DestroyCountdown);
    }
     
    public static ECSEntity SetDestroyCountdown(this ECSEntity ecsEntity,System.Single param)
    {
        var p = (GXGame.DestroyCountdown)ecsEntity.GetComponent(Components.DestroyCountdown);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.DestroyCountdown, ecsEntity);
        
        return ecsEntity;
    }
         
}