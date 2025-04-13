//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static partial class AutoSkillComponent
{
    
    public static void AddSkillComponent(this GXGame.SkillEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.SkillComponent);
    }
    
    public static void AddSkillComponent(this GXGame.SkillEntity ecsEntity,System.Int32 param)
    {
        var p  =  (GXGame.SkillComponent)ecsEntity.AddComponent(Components.SkillComponent);
        p.ID = param;
    }
          
    public static GXGame.SkillComponent GetSkillComponent(this GXGame.SkillEntity ecsEntity)
    {
        return (GXGame.SkillComponent)ecsEntity.GetComponent(Components.SkillComponent);
    }
     
    public static ECSEntity SetSkillComponent(this GXGame.SkillEntity ecsEntity,System.Int32 param)
    {
        var p = (GXGame.SkillComponent)ecsEntity.GetComponent(Components.SkillComponent);
        p.ID = param;
        ((World)ecsEntity.Parent).Reactive(Components.SkillComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetSkillComponent(this GXGame.SkillEntity ecsEntity,System.Int32 param)
    {
        var p = (GXGame.SkillComponent)ecsEntity.GetComponent(Components.SkillComponent);
        if(p==null)
        {
           p = (GXGame.SkillComponent)(ecsEntity.AddComponent(Components.SkillComponent));
        }
        p.ID = param;
        ((World)ecsEntity.Parent).Reactive(Components.SkillComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}