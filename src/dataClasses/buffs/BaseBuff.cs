using Godot;
using System;

public abstract partial class BaseBuff : Resource
{
    int _turnCount;

    public BaseBuff() : this(-1) {}
    public BaseBuff(int turnCount)
    {
        _turnCount = turnCount;
    }

    public int GetTurnCount() { return _turnCount; }
    public void AddTurnCount(int amount) { _turnCount += amount;}
    public void SetTurnCount(int newTurnCount) { _turnCount = newTurnCount; }
}
