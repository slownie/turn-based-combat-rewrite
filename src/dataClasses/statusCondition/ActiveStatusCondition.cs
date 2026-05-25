using Godot;
using System;

public partial class ActiveStatusCondition : GodotObject
{
    [Signal] public delegate void ActiveEffectEventHandler(Godot.Collections.Array<ActionEffectResource> actions);
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void StatusConditionFinishedEventHandler();

    StatusConditionResource _statusConditionResource;
    int _turnCount = -1;
    BattleConsts.TriggerType _triggerType;

    public ActiveStatusCondition() : this (null, -1) {}
    public ActiveStatusCondition(StatusConditionResource statusConditionResource, int turnCount)
    {
        _statusConditionResource = statusConditionResource;
        _turnCount = turnCount;
        _triggerType = _statusConditionResource.GetTriggerType();

        CheckForDeletion();        
    }

    /*
        Called by BattleArena after a turn has been completed.
    */
    public void DecrementTurn()
    {
        _turnCount -= 1;
        CheckForDeletion();        
    }

    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return _statusConditionResource.GetTriggerActions(); }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupActions() { return _statusConditionResource.GetCleanupActions(); }

    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int turnCount) { 
        _turnCount += turnCount; 
        CheckForDeletion();        
    }
    public void SetTurnCount(int turnCount) { 
        _turnCount = turnCount;
        CheckForDeletion();        
    }

    public BattleConsts.TriggerType GetTriggerType() { return _statusConditionResource.GetTriggerType(); }

    public Texture2D GetStatusIcon() { return _statusConditionResource.GetIcon(); }

    private void CheckForDeletion()
    {
        if (_turnCount <= 0) {
            EmitSignal(SignalName.StatusConditionFinished);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }
}
