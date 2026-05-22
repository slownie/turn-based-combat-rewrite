using Godot;
using System;

public partial class ActiveStatusCondition : GodotObject
{
    [Signal] public delegate void TurnCountEventHandler();

    StatusConditionResource _statusConditionResource;
    int _turnCount = -1;

    public ActiveStatusCondition() : this (null, -1) {}
    public ActiveStatusCondition(StatusConditionResource statusConditionResource, int turnCount)
    {
        _statusConditionResource = statusConditionResource;
        _turnCount = turnCount;
    }


    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int turnCount) { _turnCount += turnCount; }
    public void SetTurnCount(int turnCount) { _turnCount = turnCount; }
}
