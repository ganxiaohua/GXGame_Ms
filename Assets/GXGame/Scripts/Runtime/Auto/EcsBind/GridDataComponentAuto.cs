//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoGridDataComponent
{
    
    public static void AddGridDataComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.GridDataComponent);
    }
    
    public static void AddGridDataComponent(this ECSEntity ecsEntity,GXGame.GridData param)
    {
        var p  =  (GXGame.GridDataComponent)ecsEntity.AddComponent(Components.GridDataComponent);
        p.Value = param;
    }
          
    public static GXGame.GridDataComponent GetGridDataComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.GridDataComponent)ecsEntity.GetComponent(Components.GridDataComponent);
    }
     
    public static ECSEntity SetGridDataComponent(this ECSEntity ecsEntity,GXGame.GridData param)
    {
        var p = (GXGame.GridDataComponent)ecsEntity.GetComponent(Components.GridDataComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.GridDataComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetGridDataComponent(this ECSEntity ecsEntity,GXGame.GridData param)
    {
        var p = (GXGame.GridDataComponent)ecsEntity.GetComponent(Components.GridDataComponent);
        if(p==null)
        {
           p = (GXGame.GridDataComponent)(ecsEntity.AddComponent(Components.GridDataComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.GridDataComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}