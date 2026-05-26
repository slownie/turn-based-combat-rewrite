using Godot;
using System;

public partial class ActivePassiveEffect : GodotObject
{
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void TurnCountFinishedEventHandler(ActivePassiveEffect self);

    Godot.Collections.Array<ActionEffectResource> _triggerEffects;
    Godot.Collections.Array<ActionEffectResource> _cleanupEffects;

    BattleConsts.TriggerType _triggerType;

    int _turnCount = -1;
    int turnCount
    {
        get { return _turnCount; }
        set
        {
            _turnCount = value;
            if (_turnCount <= 0)
            {
                EmitSignal(SignalName.TurnCountFinished, this);
            } else {
                EmitSignal(SignalName.TurnCountChanged, _turnCount);
            }
        }
    }

    public ActivePassiveEffect() : this(null, -1, null) {}
    public ActivePassiveEffect(Godot.Collections.Array<ActionEffectResource> triggerEffects, int turnAmount, Godot.Collections.Array<ActionEffectResource> cleanupEffects=null)
    {
        _triggerEffects = triggerEffects;
        turnCount = turnAmount;
        _cleanupEffects = cleanupEffects;
    }

    /*
        Called by BattleArena after a turn has been completed.
    */
    public void DecrementTurn()
    {
        turnCount -= 1;
    }

    public int GetTurnCount() { return turnCount; }

    public Godot.Collections.Array<ActionEffectResource> GetTriggerEffects() { return _triggerEffects; }


    public BattleConsts.TriggerType GetTriggerType() { return _triggerType; }
}
