using Godot;
using System;

public partial class ActivePassiveEffect : GodotObject
{
    protected Godot.Collections.Array<ActionEffectResource> _startupEffects;
    protected Godot.Collections.Array<ActionEffectResource> _triggerEffects;
    protected Godot.Collections.Array<ActionEffectResource> _cleanupEffects;

    protected BattleConsts.TriggerType _triggerType;

    public ActivePassiveEffect() : this(null, null) {}
    public ActivePassiveEffect(Godot.Collections.Array<ActionEffectResource> triggerEffects, Godot.Collections.Array<ActionEffectResource> startupEffects=null, Godot.Collections.Array<ActionEffectResource> cleanupEffects=null)
    {
        _triggerEffects = triggerEffects;
        _startupEffects = startupEffects;
        _cleanupEffects = cleanupEffects;
    }

    public Godot.Collections.Array<ActionEffectResource> GetStartupEffects() { return _startupEffects; } 
    public Godot.Collections.Array<ActionEffectResource> GetTriggerEffects() { return _triggerEffects; }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupEffects() { return _cleanupEffects; }
    
    public BattleConsts.TriggerType GetTriggerType() { return _triggerType; }
}
