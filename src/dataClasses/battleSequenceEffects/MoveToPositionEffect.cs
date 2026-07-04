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
        Target,
        Custom
    }

    [Export] BattleArenaPosition battleArenaPosition;
    [Export] float moveSpeed = 0.5f;

    [Export] Vector2 positionOffset;

    [ExportCategory("Flags")]
    [Export] Vector2 customPosition;
    [Export] bool flipXOnStart;
    [Export] bool flipXOnEnd;

    public BattleArenaPosition GetBattleArenaPosition() { return battleArenaPosition; }
    public float GetMoveSpeed() { return moveSpeed; }

    public Vector2 GetPositionOffset() { return positionOffset; }

    public Vector2 GetCustomPosition() { return customPosition; }
    public bool GetFlipXOnStart() { return flipXOnStart; }
    public bool GetFlipXOnEnd() { return flipXOnEnd; }
}
