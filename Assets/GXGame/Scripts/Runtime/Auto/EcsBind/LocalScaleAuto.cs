//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoLocalScale
{
    
    public static void AddLocalScale(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.LocalScale);
    }
    
    public static void AddLocalScale(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p  =  (GXGame.LocalScale)ecsEntity.AddComponent(Components.LocalScale);
        p.Value = param;
    }
          
    public static GXGame.LocalScale GetLocalScale(this ECSEntity ecsEntity)
    {
        return (GXGame.LocalScale)ecsEntity.GetComponent(Components.LocalScale);
    }
     
    public static ECSEntity SetLocalScale(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.LocalScale)ecsEntity.GetComponent(Components.LocalScale);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.LocalScale, ecsEntity,EcsChangeEventState.UpdateType);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.ILocalScale) (view.Value)).LocalScale(p);
        return ecsEntity;
    }
         
}