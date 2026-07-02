using Godot;
using System;

[GlobalClass]
public partial class PassiveActionResource : Resource
{
    [Export] BattleConsts.TriggerType triggerType;

    [ExportCategory("Flags")]
    [Export] bool runOnce = false;
    [Export] bool oneShot = false;


    /*
        These actions occur when the active class is being created.
    */
    [Export] Godot.Collections.Array<ActionEffectResource> startupActions = [];

    /*
        These actions occur when the trigger type is triggered.
    */
    [Export] Godot.Collections.Array<ActionEffectResource> triggerActions = [];

    /*
        These actions occur when the turn count reaches 0 or if the effect
        is removed through some other means.
    */
    [Export] Godot.Collections.Array<ActionEffectResource> cleanupActions = [];

    public BattleConsts.TriggerType GetTriggerType() { return triggerType; }

    public Godot.Collections.Array<ActionEffectResource> GetStartupActions() { return startupActions; }
    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return triggerActions; }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupActions() { return cleanupActions; }

    public bool GetRunOnce() { return runOnce; }
    public bool GetOneShot() { return oneShot; }
}