using Godot;
using System;

public partial class ActivePassiveEffect : GodotObject
{
    [Signal] public delegate void RequestDeletionEventHandler(ActivePassiveEffect self, BattleActor user);

    protected Godot.Collections.Array<ActionEffectResource> _startupEffects;
    protected Godot.Collections.Array<ActionEffectResource> _triggerEffects;
    protected Godot.Collections.Array<ActionEffectResource> _cleanupEffects;

    protected BattleConsts.TriggerType _triggerType;
    protected bool _runOnce;
    protected bool _hasBeenRan;

    public ActivePassiveEffect() : this(null, null) {}
    public ActivePassiveEffect(Godot.Collections.Array<ActionEffectResource> triggerEffects, Godot.Collections.Array<ActionEffectResource> startupEffects=null, Godot.Collections.Array<ActionEffectResource> cleanupEffects=null)
    {
        _triggerEffects = triggerEffects;
        _startupEffects = startupEffects;
        _cleanupEffects = cleanupEffects;
        
        _hasBeenRan = false;
    }

    public Godot.Collections.Array<ActionEffectResource> GetStartupEffects() { return _startupEffects; } 
    public Godot.Collections.Array<ActionEffectResource> GetTriggerEffects() { return _triggerEffects; }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupEffects() { return _cleanupEffects; }
    
    public BattleConsts.TriggerType GetTriggerType() { return _triggerType; }
    public bool GetRunOnce() { return _runOnce; }
    public bool GetHasBeenRan() { return _hasBeenRan; }
    public void SetHasBeenRan(bool value) { _hasBeenRan = value; }
}
