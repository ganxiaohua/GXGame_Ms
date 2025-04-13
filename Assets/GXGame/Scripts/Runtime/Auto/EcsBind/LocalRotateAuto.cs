//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoLocalRotate
{
    
    public static void AddLocalRotate(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.LocalRotate);
    }
    
    public static void AddLocalRotate(this ECSEntity ecsEntity,UnityEngine.Quaternion param)
    {
        var p  =  (GXGame.LocalRotate)ecsEntity.AddComponent(Components.LocalRotate);
        p.Value = param;
    }
          
    public static GXGame.LocalRotate GetLocalRotate(this ECSEntity ecsEntity)
    {
        return (GXGame.LocalRotate)ecsEntity.GetComponent(Components.LocalRotate);
    }
     
    public static ECSEntity SetLocalRotate(this ECSEntity ecsEntity,UnityEngine.Quaternion param)
    {
        var p = (GXGame.LocalRotate)ecsEntity.GetComponent(Components.LocalRotate);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.LocalRotate, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.ILocalRotate) (view.Value)).LocalRotate(p);
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetLocalRotate(this ECSEntity ecsEntity,UnityEngine.Quaternion param)
    {
        var p = (GXGame.LocalRotate)ecsEntity.GetComponent(Components.LocalRotate);
        if(p==null)
        {
           p = (GXGame.LocalRotate)(ecsEntity.AddComponent(Components.LocalRotate));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.LocalRotate, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.ILocalRotate) (view.Value)).LocalRotate(p);
        return ecsEntity;
    } 
         
}