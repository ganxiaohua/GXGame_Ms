//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoRaycastHitMsg
{
    
    public static void AddRaycastHitMsg(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.RaycastHitMsg);
    }
    
    public static void AddRaycastHitMsg(this ECSEntity ecsEntity,System.Collections.Generic.List<UnityEngine.RaycastHit> param)
    {
        var p  =  (GXGame.RaycastHitMsg)ecsEntity.AddComponent(Components.RaycastHitMsg);
        p.Value = param;
    }
          
    public static GXGame.RaycastHitMsg GetRaycastHitMsg(this ECSEntity ecsEntity)
    {
        return (GXGame.RaycastHitMsg)ecsEntity.GetComponent(Components.RaycastHitMsg);
    }
     
    public static ECSEntity SetRaycastHitMsg(this ECSEntity ecsEntity,System.Collections.Generic.List<UnityEngine.RaycastHit> param)
    {
        var p = (GXGame.RaycastHitMsg)ecsEntity.GetComponent(Components.RaycastHitMsg);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.RaycastHitMsg, ecsEntity,EcsChangeEventState.UpdateType);
        
        return ecsEntity;
    }
         
}