using Godot;
using System;

[GlobalClass]
public partial class StatusConditionResource : Resource
{
    [Export] string statusConditionName = "";

    [Export] Texture2D icon;

    
    [Export] PassiveActionResource passiveActionResource;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public BattleConsts.TriggerType GetTriggerType() { return passiveActionResource.GetTriggerType(); }

    public Texture2D GetIcon() { return icon; }
}
