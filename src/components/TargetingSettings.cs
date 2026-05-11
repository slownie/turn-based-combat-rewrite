using Godot;
using System;

[GlobalClass]
public partial class TargetingSettings : Resource
{
    public TargetingSettings() {}

    [Export] BattleConsts.TargetType targetType = BattleConsts.TargetType.Single;
	[Export] BattleConsts.CursorMode cursorMode = BattleConsts.CursorMode.Single;
	[Export] bool targetSameSide = false;
	[Export] bool targetOppositeSide = true;

	// These could probably one variable but maybe there will be a case where we want both
	[Export] bool targetAlive = true;
	[Export] bool targetDead = false;

	public BattleConsts.TargetType GetTargetType() { return targetType; }
	public BattleConsts.CursorMode GetCursorMode() { return cursorMode; }
	public bool GetTargetSameSide() { return targetSameSide; }
	public bool GetTargetOppositeSide() { return targetOppositeSide; }
	public bool GetTargetAlive() { return targetAlive; }
	public bool GetTargetDead() { return targetDead; }
}
