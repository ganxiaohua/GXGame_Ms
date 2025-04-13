//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoCampComponent
{
    
    public static void AddCampComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.CampComponent);
    }
    
    public static void AddCampComponent(this ECSEntity ecsEntity,GXGame.Camp param)
    {
        var p  =  (GXGame.CampComponent)ecsEntity.AddComponent(Components.CampComponent);
        p.Value = param;
    }
          
    public static GXGame.CampComponent GetCampComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.CampComponent)ecsEntity.GetComponent(Components.CampComponent);
    }
     
    public static ECSEntity SetCampComponent(this ECSEntity ecsEntity,GXGame.Camp param)
    {
        var p = (GXGame.CampComponent)ecsEntity.GetComponent(Components.CampComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.CampComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetCampComponent(this ECSEntity ecsEntity,GXGame.Camp param)
    {
        var p = (GXGame.CampComponent)ecsEntity.GetComponent(Components.CampComponent);
        if(p==null)
        {
           p = (GXGame.CampComponent)(ecsEntity.AddComponent(Components.CampComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.CampComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}