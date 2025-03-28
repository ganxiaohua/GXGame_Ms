//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoFeedBackBoxComponent
{
    
    public static void AddFeedBackBoxComponent(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.FeedBackBoxComponent);
    }
    
    public static void AddFeedBackBoxComponent(this ECSEntity ecsEntity,GXGame.FeedBackBoxData param)
    {
        var p  =  (GXGame.FeedBackBoxComponent)ecsEntity.AddComponent(Components.FeedBackBoxComponent);
        p.Value = param;
    }
          
    public static GXGame.FeedBackBoxComponent GetFeedBackBoxComponent(this ECSEntity ecsEntity)
    {
        return (GXGame.FeedBackBoxComponent)ecsEntity.GetComponent(Components.FeedBackBoxComponent);
    }
     
    public static ECSEntity SetFeedBackBoxComponent(this ECSEntity ecsEntity,GXGame.FeedBackBoxData param)
    {
        var p = (GXGame.FeedBackBoxComponent)ecsEntity.GetComponent(Components.FeedBackBoxComponent);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.FeedBackBoxComponent, ecsEntity);
        
        return ecsEntity;
    }
    
    public static ECSEntity AddOrSetFeedBackBoxComponent(this ECSEntity ecsEntity,GXGame.FeedBackBoxData param)
    {
        var p = (GXGame.FeedBackBoxComponent)ecsEntity.GetComponent(Components.FeedBackBoxComponent);
        if(p==null)
        {
           p = (GXGame.FeedBackBoxComponent)(ecsEntity.AddComponent(Components.FeedBackBoxComponent));
        }
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.FeedBackBoxComponent, ecsEntity);
        
        return ecsEntity;
    } 
         
}