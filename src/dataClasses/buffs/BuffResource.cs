using Godot;
using System;

[GlobalClass]
public partial class BuffResource : Resource
{
    [Export] string buffName = "";
    [Export] Texture2D icon;

    [Export] PassiveActionResource passiveActionResource;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public BattleConsts.TriggerType GetTriggerType() { return passiveActionResource.GetTriggerType(); }


    public Texture2D GetIcon() { return icon; }

    public string GetBuffName() { return buffName; }
}
