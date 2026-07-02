using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ActorController : Node2D
{
	/*
		Required for any side effects that occur within actions.
	*/
	[Export] BattleTriggerController _battleTriggerController;

	[Export] UseableSkillResource defaultEnemyAction;


	[Signal] public delegate void EnemySkillUsedEventHandler(UseableSkillResource.SkillCostType skillCostType, int amount);

	[Signal] public delegate void EnemySelectActionEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);
	[Signal] public delegate void RandomSelectActionEventHandler(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets);
	[Signal] public delegate void EscapeSuccessEventHandler();

	Godot.Collections.Array<BattleActor> _partyActors = [];
	Godot.Collections.Array<BattleActor> _enemyActors = [];

	public void Setup(Godot.Collections.Array<BattleActor> partyActors, Godot.Collections.Array<BattleActor> enemyActors)
	{
		_partyActors = partyActors;
		_enemyActors = enemyActors;
	}

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
	public void TakeDamage(BattleActor user, BattleActor target, int damage, bool didCrit)
	{
		if (target.IsIndestructable) damage = 0;

		// Damage value should be negative
		target.AddCurHP(damage);

		RunActorSideEffects(target, BattleConsts.TriggerType.OnUserTakeDamage);

		if (target.GetCurHP() <= Mathf.RoundToInt(target.GetMaxHP() * 0.5))
		{
			RunActorSideEffects(target, BattleConsts.TriggerType.OnUserHalfHP);
		}

		if (target.GetCurHP() <= Mathf.RoundToInt(target.GetMaxHP() * 0.25))
		{
			RunActorSideEffects(target, BattleConsts.TriggerType.OnUserQuarterHP);
		}

		if (target.GetCurHP() <= 0)
		{
			RunActorSideEffects(target, BattleConsts.TriggerType.OnUserDeath);
			RunActorSideEffects(user, BattleConsts.TriggerType.OnTargetDeath);
		}

		target.EmitSignal(BattleActor.SignalName.DamageReceived, target, damage, didCrit);
	}

	public void CounterAttack(BattleActor target, int damage)
	{
		if (target.IsIndestructable) damage = 0;

		target.AddCurHP(damage);

		target.EmitSignal(BattleActor.SignalName.DamageReceived, target, damage, false);
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
			StatusConditionResource fusionStatusConditionResource = target.GetActiveStatusCondition().GetFusionStatusCondition(statusConditionResource.GetElementType());
			if (fusionStatusConditionResource != null)
			{
				ActiveStatusCondition activeStatusCondition = new ActiveStatusCondition(fusionStatusConditionResource, turnCount);
				target.SetActiveStatusCondition(activeStatusCondition);
			} else {
				GD.Print("Fusion Failed");
			}
		} else {
			ActiveStatusCondition activeStatusCondition = new ActiveStatusCondition(statusConditionResource, turnCount);
			target.SetActiveStatusCondition(activeStatusCondition);
		}
	}

	public void RemoveStatusCondition(BattleActor target)
	{
		target.RemoveStatusCondition();
	}

	public void AddBuff(BattleActor target, BuffResource buffToApply, int turnDuration)
	{
		GD.Print(target.GetActorName()+" - "+buffToApply.GetBuffName()+" - "+turnDuration+" turns - ");
		ActiveBuff buff = new ActiveBuff(buffToApply, turnDuration);
		target.AddBuff(buff);
	}

	public void AddStatModifier(BattleActor target, BattleConsts.StatBuffType statBuff, double statLevel)
	{
		switch(statBuff)
		{
			case BattleConsts.StatBuffType.Strength:
			{
				target.AddStrengthModifier(statLevel);
				break;
			}
			
			case BattleConsts.StatBuffType.Elemental:
			{
				target.AddElementalModifier(statLevel);
				break;
			}

			case BattleConsts.StatBuffType.Defense:
			{
				target.AddDefenseModifier(statLevel);
				break;
			}

			case BattleConsts.StatBuffType.Agility:
			{
				target.AddAgilityModifier(statLevel);
				break;
			}

			case BattleConsts.StatBuffType.Accuracy:
			{
				target.AddAccuraccyModifier(statLevel);
				break;
			}

			case BattleConsts.StatBuffType.Crit:
			{
				target.AddCritModifier(statLevel);
				break;
			}
		}
	}

	public void SetCharge(BattleActor target, bool enable)
	{
		target.SetCharge(enable);
	}

	public void SetFocus(BattleActor target, bool enable)
	{
		target.SetFocus(enable);
	}

	public void SetBuffIsPermanent(BattleActor target, BuffResource buffToLookFor, bool isPermanent)
	{
		target.SetBuffIsPermanent(buffToLookFor, isPermanent);
	}

	/*
		Removes all the buffs the target has.
	*/
	public void RemoveEveryBuff(BattleActor target)
	{
		target.RemoveEachBuff();
	}


	/*
		Removes all the positive buffs the target has.
	*/
	public void RemoveAllBuffs(BattleActor target)
	{
		target.RemoveAllBuffs();
	}

	/*
		Removes all the negative buffs the target has.
	*/
	public void RemoveAllDebuffs(BattleActor target)
	{
		target.RemoveAllDebuffs();
	}

	public int GetCounterDamage(BattleActor target)
	{
		return target.GetCounterDamage();
	}

	public void SetTrackDamage(BattleActor target, bool enable)
	{
		GD.Print(target.GetActorName()+" Set TrackDamage - "+enable);
		target.TrackDamage = enable;
	}

	public void ResetCounterDamage(BattleActor target)
	{
		target.ResetCounterDamage();
	}

	public void SetSelectRandomAction(BattleActor target, bool enable)
	{
		target.SelectRandomAction = enable;
	}

	public void SetQueueAction(BattleActor target, UseableActionResource queueAction)
	{
		target.SetQueueAction(queueAction);
	}

	public void RemoveQueueAction(BattleActor target)
	{
		target.RemoveQueueAction();
	}

	public void DidEscape()
	{
		EmitSignal(SignalName.EscapeSuccess); 
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

	public void RunActorSideEffects(BattleActor target, BattleConsts.TriggerType triggerType)
	{
		_battleTriggerController.RunActorSideEffects(target, triggerType);
	}

	public void AddTempo(BattleActor target, double newTempo)
	{
		target.AddTempo(newTempo);
	}

	public void SetTempo(BattleActor target, double newTempo)
	{
		target.SetTempo(newTempo);
	}

	public void AddStun(BattleActor target, double stunDuration)
	{
		target.AddStun(stunDuration);
	}

	public void SetStun(BattleActor target, double stunDuration)
	{
		target.SetStun(stunDuration);
	}

	#endregion

	#region Queries

	/*
		Return all actors whose curHP is greater than 0.
	*/
	public Godot.Collections.Array<BattleActor> GetLiveActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		IEnumerable<BattleActor> aliveQuery = battleActors.Where(battleActor => battleActor.GetCurHP() > 0);
		foreach (BattleActor actor in aliveQuery) actors.Add(actor);

		return actors;
	}

	
	/*
		Return all actors whose curHP is greater than 0 but is not equal to their maxMP.
	*/
	public Godot.Collections.Array<BattleActor> GetHurtActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		IEnumerable<BattleActor> aliveQuery = battleActors.Where(battleActor => battleActor.GetMaxMP() > battleActor.GetCurHP() && battleActor.GetCurHP() > 0);
		foreach (BattleActor actor in aliveQuery) actors.Add(actor);

		return actors;
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
	public void EnemyAISelectAction(BattleActor enemyUser)
	{
		// Action Selection
		UseableSkillResource selectedAction;

		if (GetUseableSkills(enemyUser).Count <= 0)
		{
			// Empty skill list
			selectedAction = defaultEnemyAction;
		} else {
			// Get available skills
			Godot.Collections.Array<UseableSkillResource> availableSkills = [];

			foreach(UseableSkillResource skill in GetUseableSkills(enemyUser))
			{
				if (!enemyUser.IgnoreSkillCosts)
            	{
					if (skill.GetSkillCostType() == UseableSkillResource.SkillCostType.HP)
					{
						if (enemyUser.GetCurHP() <= skill.GetSkillCostAmount() || !enemyUser.CanSelectPhysSkills)
						{
							break;
						}
					} else {
						if (enemyUser.GetCurMP() < skill.GetSkillCostAmount() || !enemyUser.CanSelectElemSkills)
						{
							break;
						}
					}
            	}
				availableSkills.Add(skill);
			}

			// No other available skills
			if (availableSkills.Count <= 0)
			{
				selectedAction = defaultEnemyAction;
			} else {
				selectedAction = availableSkills.PickRandom();
			}
		}

		// Targeting
		Godot.Collections.Array<BattleActor> _oppositeSideTargets = [];
		Godot.Collections.Array<BattleActor> _sameSideTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetUseableActionResource().GetTargetOppositeSide())
		{
			_oppositeSideTargets = _partyActors;
		}

		if (selectedAction.GetUseableActionResource().GetTargetSameSide())
		{
			_sameSideTargets = _enemyActors;
		}

		if (selectedAction.GetUseableActionResource().GetTargetSelfOnly())
		{
			_sameSideTargets.Add(enemyUser);
		}

		// 2. Are we targeting dead or alive actors?
		if (selectedAction.GetUseableActionResource().GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_sameSideTargets = GetDeadActors(_sameSideTargets);
		} else {
			// Target only alive party members
			if (_oppositeSideTargets.Count != 0) _oppositeSideTargets = GetLiveActors(_oppositeSideTargets);
			if (_sameSideTargets.Count != 0) _sameSideTargets = GetLiveActors(_sameSideTargets);
		}

		Godot.Collections.Array<BattleActor> _selectedTargets = [];
		

		switch(selectedAction.GetUseableActionResource().GetCursorMode())
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

		EmitSignal(SignalName.EnemySkillUsed, (int)selectedAction.GetSkillCostType(), selectedAction.GetSkillCostAmount());
		EmitSignal(SignalName.EnemySelectAction, selectedAction.GetUseableActionResource(), _selectedTargets);
	}

	public void SelectSetAction(BattleActor currentUser, UseableActionResource selectedAction)
	{
		// Targeting
		Godot.Collections.Array<BattleActor> _oppositeSideTargets = [];
		Godot.Collections.Array<BattleActor> _sameSideTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetTargetOppositeSide())
		{
			_oppositeSideTargets = _partyActors;
		}

		if (selectedAction.GetTargetSameSide())
		{
			_sameSideTargets = _enemyActors;
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
	
	public void SelectRandomAction(BattleActor currentUser)
	{
		GD.Print("Random Action");
		// Action Selection
		UseableActionResource selectedAction;

		if (GetUseableSkills(currentUser).Count <= 0)
		{
			selectedAction = defaultEnemyAction.GetUseableActionResource();
		} else {
			selectedAction = GetUseableSkills(currentUser).PickRandom().GetUseableActionResource();
		}

		// Targeting
		Godot.Collections.Array<BattleActor> _oppositeSideTargets = [];
		Godot.Collections.Array<BattleActor> _sameSideTargets = [];

		// Provide targeting parameters to TargetCursorController

		// 1. Are we targeting the party, enemies, both, or the self?
		if (selectedAction.GetTargetOppositeSide())
		{
			_oppositeSideTargets = _partyActors;
		}

		if (selectedAction.GetTargetSameSide())
		{
			_sameSideTargets = _enemyActors;
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
