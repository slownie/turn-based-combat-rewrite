using Godot;
using System;

public partial class ActiveStatusEffect : GodotObject
{
    int _turnCount;

    public ActiveStatusEffect(int turnCount)
    {
        _turnCount = turnCount;
    }

    public void AddTurnCount(int turnCount) { _turnCount += turnCount; }
    
}
