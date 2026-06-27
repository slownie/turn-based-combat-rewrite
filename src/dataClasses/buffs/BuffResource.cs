using Godot;
using System;

[GlobalClass]
public partial class BuffResource : Resource
{
    [Export] string buffName = "";
    [Export] Texture2D icon;

    [Export] PassiveActionResource passiveActionResource;

    [ExportCategory("Flags")]
    [Export] bool isDebuff = false; // Buff will be targeted by skills like Dekaja but ignored in skills like Dekunda
    [Export] bool isPermanent = false; // Buff will not have its turnCount decremented

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public BattleConsts.TriggerType GetTriggerType() { return passiveActionResource.GetTriggerType(); }

    public bool GetIsDebuff() { return isDebuff; }
    public bool GetIsPermanent() { return isPermanent; }
    public bool GetRunOnce() { return passiveActionResource.GetRunOnce(); }

    public Texture2D GetIcon() { return icon; }

    public string GetBuffName() { return buffName; }
}
