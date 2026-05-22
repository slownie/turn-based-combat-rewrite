using Godot;
using System;

[GlobalClass]
public partial class StatusConditionResource : Resource
{
    [Export] string statusConditionName = "";
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
}
