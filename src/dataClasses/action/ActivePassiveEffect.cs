using Godot;
using System;

public partial class ActivePassiveEffect : GodotObject
{
    [Signal] public delegate void RequestDeletionEventHandler(ActivePassiveEffect self, BattleActor user);

    protected Godot.Collections.Array<ActionEffectResource> _startupEffects;
    protected Godot.Collections.Array<ActionEffectResource> _triggerEffects;
    protected Godot.Collections.Array<ActionEffectResource> _cleanupEffects;

    protected BattleConsts.TriggerType _triggerType;
    
    /*
        Specifies whether the effect should be ran once ever.
        Use for effects you want to only activate once.
        Takes priority over oneShot.
    */
    protected bool _runOnce;
    protected bool _hasBeenRan;

    /*
        Specifies whether the effect should be ran once for the full action.
        Primarily used for actions that can activate a trigger multiple times, ie attacks.
        Loses priority to runOnce.
    */
    protected bool _oneShot; 

    public ActivePassiveEffect() : this(null, null) {}
    public ActivePassiveEffect(
        Godot.Collections.Array<ActionEffectResource> triggerEffects, 
        Godot.Collections.Array<ActionEffectResource> startupEffects=null, 
        Godot.Collections.Array<ActionEffectResource> cleanupEffects=null)
    {
        _triggerEffects = triggerEffects;
        _startupEffects = startupEffects;
        _cleanupEffects = cleanupEffects;
        
        _hasBeenRan = false;
        _oneShot = false;
    }

    public Godot.Collections.Array<ActionEffectResource> GetStartupEffects() { return _startupEffects; } 
    public Godot.Collections.Array<ActionEffectResource> GetTriggerEffects() { return _triggerEffects; }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupEffects() { return _cleanupEffects; }
    
    public BattleConsts.TriggerType GetTriggerType() { return _triggerType; }
    public bool GetRunOnce() { return _runOnce; }
    public bool GetOneShot() { return _oneShot; }

    public bool GetHasBeenRan() { return _hasBeenRan; }
    public void SetHasBeenRan(bool value) { _hasBeenRan = value; }


}
