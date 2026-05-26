using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ActorController : Node2D
{
	[Export] BattleTriggerController _battleTriggerController;

	[Export] UseableSkillResource defaultEnemyAction;


	[Signal] public delegate void EnemySelectActionEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);
	[Signal] public delegate void RandomSelectActionEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);


	#region Stats
	/*
		These are used for skills costs, NOT skills.
	*/
	public void AddActorCurHP(BattleActor target, int amount) { target.AddCurHP(amount); }
	public void SetActorCurHP(BattleActor target, int amount) { target.SetCurHP(amount); }
	public void AddActorCurMP(BattleActor target, int amount) { target.AddCurMP(amount); }
	public void SetActorCurMP(BattleActor target, int amount) { target.SetCurMP(amount); }
	#endregion

	#region Actions
	public void TakeDamage(BattleActor target, int damage, bool didCrit)
	{
		if (target.IsIndestructable) damage = 0;

		target.AddCurHP(damage);
		target.EmitSignal(BattleActor.SignalName.DamageReceived, target, damage, didCrit);
	}

	public void TakeHeal(BattleActor target, int heal)
	{
		target.AddCurHP(heal);
		target.EmitSignal(BattleActor.SignalName.HealReceived, target, heal);
	}

	public void TakeRejuvenate(BattleActor target, int rejuvenate)
	{
		target.AddCurMP(rejuvenate);
		target.EmitSignal(BattleActor.SignalName.RejuvenateReceived, target, rejuvenate);
	}

	public void ActionMissed(BattleActor target)
	{
		target.EmitSignal(BattleActor.SignalName.MissReceived, target);
	}

	public void SetStatusCondition(BattleActor target, StatusConditionResource statusConditionResource, int turnCount)
	{
		if (target.GetActiveStatusCondition() != null)
		{
			// Fusion Status Conditions
		} else {
			ActiveStatusCondition activeStatusCondition = new ActiveStatusCondition(statusConditionResource, turnCount);
			
			target.SetActiveStatusCondition(activeStatusCondition);
		}
	}

	public void AddBuff(BattleActor target, BuffResource buffToApply, int turnDuration)
	{
		ActiveBuff buff = new ActiveBuff(buffToApply, turnDuration);
		target.AddBuff(buff);
	}

	public void RemoveBuff(BattleActor target)
	{
		
	}

	public void RemoveAllBuffs(BattleActor target)
	{
		
	}

	public void RemoveAllDebuffs(BattleActor target)
	{
		
	}

	public void SetSelectRandomAction(BattleActor target, bool enable)
	{
		target.SelectRandomAction = enable;
	}

	public void SetImmortality(BattleActor target, bool enable)
	{
		GD.Print(target.GetActorName()+" Set Immortality - "+enable);
		target.IsImmortal = enable;
	}

	public void SetIndestructable(BattleActor target, bool enable)
	{
		target.IsIndestructable = enable;
	}

	public void SetMenuEntry(BattleActor target, BattleConsts.MenuEntryType menuEntryType, bool enable)
	{
		switch(menuEntryType)
		{
			case BattleConsts.MenuEntryType.SkillPhysical:
			{
				target.CanSelectPhysSkills = enable;
				break;
			}

			case BattleConsts.MenuEntryType.SkillElemental:
			{
				target.CanSelectElemSkills = enable;
				break;
			}

			case BattleConsts.MenuEntryType.Item:
			{
				target.CanSelectItems = enable;
				break;
			}
		}
	}

	public void SetIgnoreSkillCosts(BattleActor target, bool ignore)
	{
		target.IgnoreSkillCosts = ignore;
	}

	public void SetIgnoreAffinity(BattleActor target, bool ignore)
	{
		target.IgnoreAffinity = ignore;
	}

	public void SetSkillSuccessGuarnatee(BattleActor target, bool guarantee)
	{
		target.SkillSuccessGuarantee = guarantee;
	}


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
		// Action Selection
		UseableActionResource selectedAction;

		if (GetUseableSkills(enemyUser).Count <= 0)
		{
			selectedAction = defaultEnemyAction.GetUseableActionResource();
		} else {
			int selectedActionIndex = (int)(GD.Randi() % GetUseableSkills(enemyUser).Count - 1);
			selectedAction = GetUseableSkills(enemyUser)[selectedActionIndex].GetUseableActionResource();
		}

		// Targeting
		Godot.Collections.Array<BattleActor> _oppositeSideTargets = [];
		Godot.Collections.Array<BattleActor> _sameSideTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetTargetOppositeSide())
		{
			_oppositeSideTargets = GetPartyMembers(battleActors);
		}

		if (selectedAction.GetTargetSameSide())
		{
			_sameSideTargets = GetEnemies(battleActors);
		}

		if (selectedAction.GetTargetSelfOnly())
		{
			_sameSideTargets.Add(enemyUser);
		}

		// 2. Are we targeting dead or alive actors?
		if (selectedAction.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_sameSideTargets = GetDeadActors(_sameSideTargets);
		} else {
			// Target only alive party members
			if (_oppositeSideTargets.Count != 0) _oppositeSideTargets = GetLiveActors(_oppositeSideTargets);
			if (_sameSideTargets.Count != 0) _sameSideTargets = GetLiveActors(_sameSideTargets);
		}

		Godot.Collections.Array<BattleActor> _selectedTargets = [];
		

		switch(selectedAction.GetCursorMode())
		{
			case BattleConsts.CursorMode.Single:
			{
				// Pick a random target
				if (_sameSideTargets.Count != 0)
				{
					_selectedTargets.Add(_sameSideTargets.PickRandom());
				} else {
					_selectedTargets.Add(_oppositeSideTargets.PickRandom());
				}
				break;
			}

			case BattleConsts.CursorMode.Side:
			{
				if (_sameSideTargets.Count != 0)
				{
					_selectedTargets = _sameSideTargets;
				} else {
					_selectedTargets = _oppositeSideTargets;
				}
				break;
			}

			case BattleConsts.CursorMode.All:
			{
				_selectedTargets.AddRange(_oppositeSideTargets);
				_selectedTargets.AddRange(_sameSideTargets);
				break;
			}
		}

		EmitSignal(SignalName.EnemySelectAction, selectedAction, _selectedTargets);
	}
	
	public void SelectRandomAction(BattleActor currentUser, Godot.Collections.Array<BattleActor> battleActors)
	{
		GD.Print("Random Action");
		// Action Selection
		UseableActionResource selectedAction;

		if (GetUseableSkills(currentUser).Count <= 0)
		{
			selectedAction = defaultEnemyAction.GetUseableActionResource();
		} else {
			int selectedActionIndex = (int)(GD.Randi() % GetUseableSkills(currentUser).Count - 1);
			selectedAction = GetUseableSkills(currentUser)[selectedActionIndex].GetUseableActionResource();
		}

		// Targeting
		Godot.Collections.Array<BattleActor> _oppositeSideTargets = [];
		Godot.Collections.Array<BattleActor> _sameSideTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetTargetOppositeSide())
		{
			_oppositeSideTargets = GetPartyMembers(battleActors);
		}

		if (selectedAction.GetTargetSameSide())
		{
			_sameSideTargets = GetEnemies(battleActors);
		}

		if (selectedAction.GetTargetSelfOnly())
		{
			_sameSideTargets.Add(currentUser);
		}

		// 2. Are we targeting dead or alive actors?
		if (selectedAction.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_sameSideTargets = GetDeadActors(_sameSideTargets);
		} else {
			// Target only alive party members
			if (_oppositeSideTargets.Count != 0) _oppositeSideTargets = GetLiveActors(_oppositeSideTargets);
			if (_sameSideTargets.Count != 0) _sameSideTargets = GetLiveActors(_sameSideTargets);
		}

		Godot.Collections.Array<BattleActor> _selectedTargets = [];
		

		switch(selectedAction.GetCursorMode())
		{
			case BattleConsts.CursorMode.Single:
			{
				// Pick a random target
				if (_sameSideTargets.Count != 0)
				{
					int selectedTargetIndex = (int)(GD.Randi() % (_sameSideTargets.Count - 1));
					_selectedTargets.Add(_sameSideTargets[selectedTargetIndex]);
				} else {
					int selectedTargetIndex = (int)(GD.Randi() % (_oppositeSideTargets.Count - 1));
					_selectedTargets.Add(_oppositeSideTargets[selectedTargetIndex]);
				}
				break;
			}

			case BattleConsts.CursorMode.Side:
			{
				if (_sameSideTargets.Count != 0)
				{
					_selectedTargets = _sameSideTargets;
				} else {
					_selectedTargets = _oppositeSideTargets;
				}
				break;
			}

			case BattleConsts.CursorMode.All:
			{
				_selectedTargets.AddRange(_oppositeSideTargets);
				_selectedTargets.AddRange(_sameSideTargets);
				break;
			}
		}

		EmitSignal(SignalName.EnemySelectAction, selectedAction, _selectedTargets);
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
