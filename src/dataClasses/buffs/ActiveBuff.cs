using Godot;
using System;

public partial class ActiveBuff : GodotObject
{
    [Signal] public delegate void TurnCountChangedEventHandler(int currentTurnCount);
    [Signal] public delegate void BuffFinishedEventHandler(ActiveBuff activeBuff);

    BuffResource _buffResource;
    int _turnCount = -1;

    public ActiveBuff() : this (null, -1) {}
    public ActiveBuff(BuffResource buffResource, int turnCount)
    {
        _buffResource = buffResource;
        _turnCount = turnCount;

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

    // USED FOR TYPE CHECKING
    public BuffResource GetBuffResource() { return _buffResource; }

    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return _buffResource.GetBuffActions(); }

    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int turnCount) { 
        _turnCount += turnCount; 
        CheckForDeletion();        
    }
    public void SetTurnCount(int turnCount) { 
        _turnCount = turnCount;
        CheckForDeletion();        
    }

    public Texture2D GetStatusIcon() { return _buffResource.GetIcon(); }

    private void CheckForDeletion()
    {
        if (_turnCount <= 0) {
            EmitSignal(SignalName.BuffFinished, this);
        } else {
            EmitSignal(SignalName.TurnCountChanged, _turnCount);
        }
    }
}
