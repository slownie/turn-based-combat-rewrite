using Godot;
using System;

public partial class ActiveStatusCondition : GodotObject
{
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void StatusConditionFinishedEventHandler();

    StatusConditionResource _statusConditionResource;
    int _turnCount = -1;

    public ActiveStatusCondition() : this (null, -1) {}
    public ActiveStatusCondition(StatusConditionResource statusConditionResource, int turnCount)
    {
        _statusConditionResource = statusConditionResource;
        _turnCount = turnCount;

        // Signal for deletion, should not need to be here but nevertheless
        if (_turnCount <= 0) {
            EmitSignal(SignalName.StatusConditionFinished);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }

    /*
        Called by BattleArena after a turn has been completed.
    */
    public void DecrementTurn()
    {
        _turnCount -= 1;

        // Signal for deletion
        if (_turnCount <= 0) {
            EmitSignal(SignalName.StatusConditionFinished);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }

    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return _statusConditionResource.GetTriggerActions(); }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupActions() { return _statusConditionResource.GetCleanupActions(); }

    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int turnCount) { 
        _turnCount += turnCount; 

        // Signal for deletion
        if (_turnCount <= 0) {
            EmitSignal(SignalName.StatusConditionFinished);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }
    public void SetTurnCount(int turnCount) { 
        _turnCount = turnCount;

        // Signal for deletion
        if (_turnCount <= 0) {
            EmitSignal(SignalName.StatusConditionFinished);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }

    public BattleConsts.TriggerType GetTriggerType() { return _statusConditionResource.GetTriggerType(); }

    public Texture2D GetStatusIcon() { return _statusConditionResource.GetIcon(); }

}
