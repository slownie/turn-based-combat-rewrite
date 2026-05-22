using Godot;
using System;

public partial class ActiveStatusCondition : GodotObject
{
    BattleConsts.StatusCondition _statusCondition = BattleConsts.StatusCondition.None;
    int _turnCount = -1;


    public ActiveStatusCondition() {}

    public BattleConsts.StatusCondition GetStatusCondition() { return _statusCondition; }
    public void SetStatusCondition(BattleConsts.StatusCondition statusCondition) { _statusCondition = statusCondition; }

    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int turnCount) { _turnCount += turnCount; }
    public void SetTurnCount(int turnCount) { _turnCount = turnCount; }
}
