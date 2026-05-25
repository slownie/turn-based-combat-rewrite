using Godot;
using System;

[GlobalClass]
public partial class BuffResource : Resource
{
    [Export] string buffName = "";
    [Export] Texture2D icon;

    [Export] BattleConsts.TriggerType triggerType;
    [Export] Godot.Collections.Array<ActionEffectResource> buffActions = [];


    public BattleConsts.TriggerType GetTriggerType() { return triggerType; }
    public Godot.Collections.Array<ActionEffectResource> GetBuffActions() { return buffActions; }

    public Texture2D GetIcon() { return icon; }
}
