using Godot;
using System;

[GlobalClass]
public partial class MoveToPositionEffect : BattleSequenceEffectResource
{
    public enum BattleArenaPosition
    {
        None,
        Initial,
        Center,
    
    }

    [Export] BattleArenaPosition battleArenaPosition;
    [ExportCategory("Flags")]
    [Export] Vector2 customPosition;
    [Export] bool moveToTarget;

    BattleArenaPosition GetBattleArenaPosition() { return battleArenaPosition; }
    Vector2 GetCustomPosition() { return customPosition; }
    bool GetMoveToTarget() { return moveToTarget; }

}
