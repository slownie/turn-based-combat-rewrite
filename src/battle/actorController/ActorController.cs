using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ActorController : Node2D
{
	[Export] UseableSkillResource defaultEnemyAction;
	[Signal] public delegate void EnemySelectActionEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);

	#region Stats
	public void AddActorCurHP(BattleActor target, int amount) { target.AddCurHP(amount); }
	public void SetActorCurHP(BattleActor target, int amount) { target.SetCurHP(amount); }
	public void AddActorCurMP(BattleActor target, int amount) { target.AddCurMP(amount); }
	public void SetActorCurMP(BattleActor target, int amount) { target.SetCurMP(amount); }
	#endregion

	#region Queries
	public Godot.Collections.Array<BattleActor> GetLiveActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		IEnumerable<BattleActor> aliveQuery = battleActors.Where(battleActor => battleActor.GetCurHP() > 0);
		foreach (BattleActor actor in aliveQuery) actors.Add(actor);

		return actors;
	}

	public Godot.Collections.Array<BattleActor> GetHurtActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		return null;
	}

	/*
		curMP < maxMP
		Could not come up with a better word than depleted so roll with it
	*/
	public Godot.Collections.Array<BattleActor> GetDepletedActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		return null;
	}

	// HasBuff, HasCondition, HasDebuff, etc

	public Godot.Collections.Array<BattleActor> GetDeadActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		IEnumerable<BattleActor> deadQuery = battleActors.Where(battleActor => battleActor.GetCurHP() == 0);
		foreach (BattleActor actor in deadQuery) actors.Add(actor);

		return actors;
	}

	public Godot.Collections.Array<BattleActor> GetPartyMembers(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		foreach (BattleActor actor in battleActors)
		{
			if (actor.GetIsPlayer()) actors.Add(actor);
		} 
		return actors;
	}

	public Godot.Collections.Array<BattleActor> GetEnemies(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		foreach (BattleActor actor in battleActors)
		{
			if (!actor.GetIsPlayer()) actors.Add(actor);
		} 
		return actors;
	}

	public Godot.Collections.Array<UseableSkillResource> GetUseableSkills(BattleActor battleActor)
	{
		Godot.Collections.Array<UseableSkillResource> useableSkills = [];
		foreach (BaseSkillResource baseSkill in battleActor.GetSkills())
		{
			if (baseSkill is UseableSkillResource) useableSkills.Add((UseableSkillResource)baseSkill);
		}
		return useableSkills;
	}
	#endregion

	#region Enemy AI
	public void EnemyAISelectAction(BattleActor enemyUser, Godot.Collections.Array<BattleActor> battleActors)
	{
		// Easier naming
		UseableActionResource selectedAction;

		if (GetUseableSkills(enemyUser).Count <= 0)
		{
			selectedAction = defaultEnemyAction.GetUseableActionResource();
		} else {
			int selectedActionIndex = (int)(GD.Randi() % GetUseableSkills(enemyUser).Count - 1);
			selectedAction = GetUseableSkills(enemyUser)[selectedActionIndex].GetUseableActionResource();
		}

		Godot.Collections.Array<BattleActor> _partyTargets = [];
		Godot.Collections.Array<BattleActor> _enemyTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetTargetOppositeSide())
		{
			_enemyTargets = GetEnemies(battleActors);
		}

		if (selectedAction.GetTargetSameSide())
		{
			_partyTargets = GetPartyMembers(battleActors);
		}

		if (selectedAction.GetTargetSelfOnly())
		{
			_partyTargets.Add(enemyUser);
		}

		// 2. Are we targeting dead or alive actors?
		if (selectedAction.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_partyTargets = GetDeadActors(_partyTargets);
		} else {
			if (_enemyTargets.Count != 0) _enemyTargets = GetLiveActors(_enemyTargets);
			if (_partyTargets.Count != 0) _partyTargets = GetLiveActors(_partyTargets);
		}

		EmitSignal(SignalName.EnemySelectAction, selectedAction, battleActors);
	}
	#endregion

	#region Sequences

	/*
		Keep in mind this moves the SPRITE'S OFFSET NOT THE POSITION OF THE ACTOR
	*/
	public void MoveActor(BattleActor actor, Vector2 movePosition)
	{
	}

	public void SetActorSprite(BattleActor actor, string spriteName)
	{
		
	}

	#endregion

	
}
