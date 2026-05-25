using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillResource : BaseSkillResource
{
    [Export] BattleConsts.TriggerType triggerType;

    [ExportCategory("Actions")]
    /*
        These actions occur when the StatusCondition's trigger type is triggered.
    */
    [Export] Godot.Collections.Array<ActionEffectResource> triggerActions = [];

    /*
        These actions occur when the StatusCondition's turn count reaches 0 or if the StatusCondition
        is removed through some other means.
    */
    [Export] Godot.Collections.Array<ActionEffectResource> cleanupActions = [];

    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return triggerActions; }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupActions() { return cleanupActions; }


    public BattleConsts.TriggerType GetTriggerType() { return triggerType; }

    
}
