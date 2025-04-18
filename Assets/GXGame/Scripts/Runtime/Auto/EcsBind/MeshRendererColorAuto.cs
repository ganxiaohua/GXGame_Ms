//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoMeshRendererColor
{
    
    public static void AddMeshRendererColor(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.MeshRendererColor);
    }
    
    public static void AddMeshRendererColor(this ECSEntity ecsEntity,UnityEngine.Color param)
    {
        var p  =  (GXGame.MeshRendererColor)ecsEntity.AddComponent(Components.MeshRendererColor);
        p.Value = param;
    }
          
    public static GXGame.MeshRendererColor GetMeshRendererColor(this ECSEntity ecsEntity)
    {
        return (GXGame.MeshRendererColor)ecsEntity.GetComponent(Components.MeshRendererColor);
    }
     
    public static ECSEntity SetMeshRendererColor(this ECSEntity ecsEntity,UnityEngine.Color param)
    {
        var p = (GXGame.MeshRendererColor)ecsEntity.GetComponent(Components.MeshRendererColor);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.MeshRendererColor, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.IMeshRendererColor) (view.Value)).MeshRendererColor(p);
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetMeshRendererColor(this ECSEntity ecsEntity,UnityEngine.Color param)
    {
        var p = (GXGame.MeshRendererColor)ecsEntity.GetComponent(Components.MeshRendererColor);
        if(p==null)
        {
           p = (GXGame.MeshRendererColor)(ecsEntity.AddComponent(Components.MeshRendererColor));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.MeshRendererColor, ecsEntity);
        View view = ecsEntity.GetView();
        if (view == null) return null;
        ((GXGame.IMeshRendererColor) (view.Value)).MeshRendererColor(p);
        return ecsEntity;
    } 
         
}