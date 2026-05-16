using Godot;
using System;

[GlobalClass]
public partial class UseableActionResource : Resource
{
	[Export] Godot.Collections.Array<ActionEffectResource> actions = [];

	[Export] BattleConsts.TargetType targetType = BattleConsts.TargetType.Single;
	[Export] BattleConsts.CursorMode cursorMode = BattleConsts.CursorMode.Single;

	[Export] bool targetOppsiteSide = true;
	[Export] bool targetSameSide = false; 
	[Export] bool targetSelfOnly = false;

	// Skill/Stat/Condition related parameters
	[Export] bool targetHurtOnly = false;
	[Export] bool targetDepletedOnly = false;
	[Export] bool targetDeadOnly = false; 

	[Export] bool targetHasBuffOnly = false;
	[Export] bool targetHasDebuffOnly = false;
	[Export] bool targetHasStatusOnly = false;



	// Todo, specify enable conditions in UseableSkill/UseableItem
	
	public Godot.Collections.Array<ActionEffectResource> GetActions() { return actions; }
	public BattleConsts.TargetType GetTargetType() { return targetType; }
	public BattleConsts.CursorMode GetCursorMode() { return cursorMode; }
	
	/*
		Note that this is RELATIVE to the user.
			- In the UIMenuController, SameSide=Party, OppositeSide=Enemies
			- In EnemyAI, SameSide=Enemies, OppositeSide=Party
	*/
	public bool GetTargetOppositeSide() { return targetOppsiteSide; }
	public bool GetTargetSameSide() { return targetSameSide; }
	public bool GetTargetSelfOnly() { return targetSelfOnly; }
	public bool GetTargetDeadOnly() { return targetDeadOnly; }
	
}
