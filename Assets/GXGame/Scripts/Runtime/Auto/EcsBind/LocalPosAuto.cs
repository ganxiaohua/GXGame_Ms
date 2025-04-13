//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoLocalPos
{
    
    public static void AddLocalPos(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.LocalPos);
    }
    
    public static void AddLocalPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p  =  (GXGame.LocalPos)ecsEntity.AddComponent(Components.LocalPos);
        p.Value = param;
    }
          
    public static GXGame.LocalPos GetLocalPos(this ECSEntity ecsEntity)
    {
        return (GXGame.LocalPos)ecsEntity.GetComponent(Components.LocalPos);
    }
     
    public static ECSEntity SetLocalPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.LocalPos)ecsEntity.GetComponent(Components.LocalPos);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.LocalPos, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.ILocalPosition) (view.Value)).LocalPosition(p);
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetLocalPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
    {
        var p = (GXGame.LocalPos)ecsEntity.GetComponent(Components.LocalPos);
        if(p==null)
        {
           p = (GXGame.LocalPos)(ecsEntity.AddComponent(Components.LocalPos));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.LocalPos, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.ILocalPosition) (view.Value)).LocalPosition(p);
        return ecsEntity;
    } 
         
}