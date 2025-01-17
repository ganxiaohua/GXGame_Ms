//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoHP
{
    
    public static void AddHP(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.HP);
    }
    
    public static void AddHP(this ECSEntity ecsEntity,System.Int32 param)
    {
        var p  =  (GXGame.HP)ecsEntity.AddComponent(Components.HP);
        p.Value = param;
    }
          
    public static GXGame.HP GetHP(this ECSEntity ecsEntity)
    {
        return (GXGame.HP)ecsEntity.GetComponent(Components.HP);
    }
     
    public static ECSEntity SetHP(this ECSEntity ecsEntity,System.Int32 param)
    {
        var p = (GXGame.HP)ecsEntity.GetComponent(Components.HP);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.HP, ecsEntity);
        
        return ecsEntity;
    }
         
}