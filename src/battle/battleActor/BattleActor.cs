using Godot;
using System;

public partial class BattleActor : Node2D
{
	[Export] PackedScene buffScene;

	// Stat Signals
	[Signal] public delegate void HPChangedEventHandler(int newHP);
	[Signal] public delegate void HPDepletedEventHandler();

	[Signal] public delegate void MPChangedEventHandler(int newMP);

	// Skill/Action Related Signals
	[Signal] public delegate void DamageReceivedEventHandler(BattleActor actor, int value, bool isCrit);
	[Signal] public delegate void HealReceivedEventHandler(BattleActor actor, int value);
	[Signal] public delegate void RejuvenateReceivedEventHandler(BattleActor actor, int value);

	[Signal] public delegate void StatusConditionReceivedEventHandler(BattleActor actor, StatusConditionResource statusConditionResource);
	[Signal] public delegate void StatusConditionRemovedEventHandler(BattleActor actor, StatusConditionResource statusConditionResource);

	[Signal] public delegate void MissReceivedEventHandler(BattleActor actor);

	// Readiness Signals
	[Signal] public delegate void ReadinessChangedEventHandler(double readiness);
	[Signal] public delegate void ReadyToActEventHandler(BattleActor battleActor);

	// Side Effect Related Signals
	[Signal] public delegate void AddSideEffectEventHandler(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect);
	[Signal] public delegate void RemoveSideEffectEventHandler(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect);

	const double _modifierMin = 0.25;
	const double _modifierMax = 2;


	string _actorName = "Placeholder";
	int _fusionID = -1;
	CharacterStats _characterStats;

	// EQUIPMENT HERE

	Godot.Collections.Array<BaseSkillResource> _skills = [];
	Godot.Collections.Array<UseableSkillResource> _useableSkills = [];
	Godot.Collections.Array<ActivePassiveSkill> _passiveSkills = [];
	Godot.Collections.Array<FusionSkillResource> _fusionSkills = [];

	ActiveStatusCondition _activeStatusCondition = null;

	Godot.Collections.Array<ActiveBuff> _buffs = [];

	double _strengthModifier = 1;
	double _elementalModifier = 1;
	double _defenseModifier = 1;
	double _agilityModifier = 1;
	double _accuraccyModifier = 1;
	double _critModifier = 1;
	
	bool _chargeEnabled = false;
	bool _focusEnabled = false;



	/*
	Godot.Collections.Dictionary<BattleConsts.ElementType, double> _attackElementModifiers = new Godot.Collections.Dictionary<BattleConsts.ElementType, double>()
	{
		{BattleConsts.ElementType.Phys, 1},	
		{BattleConsts.ElementType.Fire, 1},
		{BattleConsts.ElementType.Water, 1},
		{BattleConsts.ElementType.Wind, 1},
		{BattleConsts.ElementType.Earth, 1},
		{BattleConsts.ElementType.Steam, 1},
		{BattleConsts.ElementType.Electric, 1},
		{BattleConsts.ElementType.Metal, 1},
		{BattleConsts.ElementType.Ice, 1},
		{BattleConsts.ElementType.Life, 1},
		{BattleConsts.ElementType.Gravity, 1},
	};
	*/

	CharacterAffinity _characterAffinity;

	UseableActionResource _queueAction;
	int _counterDamage = 0;


	bool _isPlayer = true;

	BattleActorAnim _battleActorAnim;
	Texture2D _battleIcon;


	// Test UI
	TextureRect _statusIcon;
	Label _statusTurnLabel;

	Label _curHPLabel;
	Label _maxHPLabel;

	Label _curMPLabel;
	Label _maxMPLabel;

	Label _tempoLabel;

	VBoxContainer _buffContainer;

	#region Properties
	bool isActive = true;
	public bool IsActive
	{
		get {return isActive;}
		set
		{
			isActive = value;
			SetProcess(isActive);
		}
	}

	public double TimeScale {get; set;} = 1.0;

	// The default ready gain
	double _battleSpeed = 7;

	// When this value reaches '100.0', the battler is ready to take their turn
	double readiness = 0.0;
	double Readiness
	{
		get {return readiness;}
		set
		{
			readiness = value;
			EmitSignal(SignalName.ReadinessChanged, readiness);

			if (readiness >= 100.0)
			{
				readiness = 100.0;
				EmitSignal(SignalName.ReadyToAct, this);
				SetProcess(false);
			}
		}
	}

	double tempo = 1.0;
	double Tempo
	{
		get { return tempo; }
		set
		{
			tempo = value;
			if (tempo < _modifierMin) tempo = _modifierMin;
			if (_modifierMax < tempo) tempo = _modifierMax;
		}
	}
	const double _tempoRate = 0.2;

	double stun = 0.0;
	double Stun
	{
		get { return stun; }
		set
		{
			stun = value;
			if (stun < 0.0) stun = 0.0;
		}
	}

	// If 'false' this actor cannot be targeted by any action
	public bool IsTargetable {get; set;} = true;


	// Status Effect Parameters

	/*
		When true, the user will select a random action.
		Primarily used by the Confuse status effect.
	*/
	public bool SelectRandomAction {get; set; } = false; 

	/*
		When true, the user will track the damage dealt to it.
		Primarily used by the Frostbite status effect.
	*/
	public bool TrackDamage {get; set; } = false;

	/*
		When true, user's HP will never reach 0.
		Primarily used by the Immortality status effect.
	*/
	public bool IsImmortal {get; set; } = false;

	/*
		When true, any damage taken by this user will be reduced to 0.
		Unused for now.
	*/
	public bool IsIndestructable {get; set; } = false;

	/*
		When true, user can select skills that require HP to use.
		Primarily used by the Break status effect.
	*/
	public bool CanSelectPhysSkills {get; set;} = true;

	/*
		When true, user can select skills that require MP to use.
		Primarily used by the Seal status effect.
	*/
	public bool CanSelectElemSkills {get; set;} = true;

	/*
		When true, user can select items.
		Unused for now.
	*/
	public bool CanSelectItems {get; set;} = true;

	/*
		When true, user can select any skill regardless of the skill's cost.
		The subtraction of the cost will also not occur.
		Primarily used by the Overdrive status effect.
	*/
	public bool IgnoreSkillCosts {get; set;} = false;

	/*
		When true, user's attacks will ignore the target's affinity.
		Primarily used by the Cleave status effect.
	*/
	public bool IgnoreAffinity {get; set;} = false;

	/*
		When true, all additional effects from a skill or item will occur.
		Primarily used by the Elevate status effect.
	*/
	public bool SkillSuccessGuarantee {get; set;} = false; // Elevate

	/*
	
	*/

	#endregion
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_statusIcon = GetNode<TextureRect>("TestUI/StatusContainer/StatusIcon");
		_statusTurnLabel = GetNode<Label>("TestUI/StatusContainer/StatusTurn");

		_curHPLabel = GetNode<Label>("TestUI/StatContainer/HPContainer/CurHP");
		_maxHPLabel = GetNode<Label>("TestUI/StatContainer/HPContainer/MaxHP");
		_curMPLabel = GetNode<Label>("TestUI/StatContainer/MPContainer/CurMP");
		_maxMPLabel = GetNode<Label>("TestUI/StatContainer/MPContainer/MaxMP");

		_tempoLabel = GetNode<Label>("TestUI/Tempo");

		IsActive = false;
	}

	public override void _Process(double delta)
	{
		if (Tempo != 1.0)
		{
			// Tempo
			if (Tempo < 1.01) Tempo += (_tempoRate * TimeScale * delta);
			if (1.01 < Tempo) Tempo -= (_tempoRate * TimeScale * delta);

			_tempoLabel.Text = Tempo.ToString();
		}

		if (0.0 < Stun)
		{
			// Stun
			Stun -= _battleSpeed * TimeScale * delta;
		} else {
			// Readiness
			Readiness += _battleSpeed * (_characterStats.GetAgility() * _agilityModifier * Tempo) * TimeScale * delta;
		}
	}

	public void Setup(
		int x, 
		int y, 
		string actorName, 
		int fusionID,
		CharacterStats characterStats, 
		Godot.Collections.Array<BaseSkillResource> skills,
		Godot.Collections.Array<FusionSkillResource> fusionSkills,
		CharacterAffinity characterAffinity,

		PackedScene battleActorAnim, 
		Texture2D battleIcon,
		bool isPlayer
	)
	{
		Vector2 newPosition = new Vector2(x, y);
		Position = newPosition;

		_actorName = actorName;
		_fusionID = fusionID;
		
		_characterStats = characterStats;
		_characterStats.HPDepleted += OnStatsHPDepleted;
		_characterStats.HPRevive += OnActorRevived;

		_activeStatusCondition = null;

		_skills = skills;

		// Skill sorting
		foreach (BaseSkillResource skill in _skills)
		{
			if (skill is UseableSkillResource)
			{
				_useableSkills.Add(skill as UseableSkillResource);
			}

			// Create passive skills
			if (skill is PassiveSkillResource)
			{
				// Unfortunately these have to be connected in another function due to execution order
				PassiveSkillResource _passiveSkill = skill as PassiveSkillResource;
				ActivePassiveSkill activePassiveSkill = new ActivePassiveSkill(_passiveSkill);
				_passiveSkills.Add(activePassiveSkill);
			}
		}

		_fusionSkills = fusionSkills;

		_characterAffinity = characterAffinity;

		_curHPLabel.Text = _characterStats.GetCurHP().ToString();
		_maxHPLabel.Text = _characterStats.GetMaxHP().ToString();
		_curMPLabel.Text = _characterStats.GetCurMP().ToString();
		_maxMPLabel.Text = _characterStats.GetMaxMP().ToString();

		// Animation
		_battleActorAnim = battleActorAnim.Instantiate() as BattleActorAnim;
		AddChild(_battleActorAnim);

		_battleActorAnim.PlayAnimation("idle");

		_battleIcon = battleIcon;

		_isPlayer = isPlayer;
		_battleActorAnim.Setup(_isPlayer);
	}

	public void TurnEnd()
	{
		if (_activeStatusCondition != null)
		{
			_activeStatusCondition.DecrementTurn();
		}

		foreach(ActiveBuff activeBuff in _buffs)
		{
			activeBuff.DecrementTurn();
		}
	}

	public void ConnectPassiveSkills()
	{
		foreach(ActivePassiveSkill activePassiveSkill in _passiveSkills)
		{
			EmitSignal(SignalName.AddSideEffect, this, (int)activePassiveSkill.GetTriggerType(), activePassiveSkill);
		}
	}

	#region Getters

	public string GetActorName() { return _actorName; }

	// Stats
	public CharacterStats GetCharacterStats() { return _characterStats; } // Primarily used to expose signals
	public int GetCurHP() { return _characterStats.GetCurHP(); }
	public int GetMaxHP() { return _characterStats.GetMaxHP(); }
	public int GetCurMP() { return _characterStats.GetCurMP(); }
	public int GetMaxMP() { return _characterStats.GetMaxMP(); }

	public void AddCurHP(int amount) {
		// This should also prevent OnDeath effects from activating since 
		// HP never reaches 0 which would cause those effects to activate.

		// Counter damage
		if (TrackDamage)
		{
			_counterDamage += amount;
			if (_counterDamage > 0) _counterDamage = 0;
		}

		// Gross code but this works
		if (IsImmortal)
		{
			// Would this kill the actor? (amount is negative)
			if (_characterStats.GetCurHP() + amount <= 0)
			{
				_characterStats.SetCurHP(1);
			} else {
				_characterStats.AddCurHP(amount); 
			}
		} else {
			_characterStats.AddCurHP(amount); 
		}
		
		_curHPLabel.Text = _characterStats.GetCurHP().ToString();
	}
	public void SetCurHP(int amount) { 
		_characterStats.SetCurHP(amount); 
		_curHPLabel.Text = _characterStats.GetCurHP().ToString();	
	}
	public void AddCurMP(int amount) { 
		_characterStats.AddCurMP(amount); 
		_curMPLabel.Text = _characterStats.GetCurMP().ToString();
	}
	public void SetCurMP(int amount) { 
		_characterStats.SetCurMP(amount); 
		_curMPLabel.Text = _characterStats.GetCurMP().ToString();
	}

	public int GetStrength() { return _characterStats.GetStrength(); }
	public int GetElemental() { return _characterStats.GetElemental(); }
	public int GetAgility() { return _characterStats.GetAgility(); }
	public int GetLuck() { return _characterStats.GetLuck(); }
	public int GetDefense() { return _characterStats.GetDefense(); }
	public int GetResistance() { return _characterStats.GetResistance(); }

	public int GetFusionID() { return _fusionID; }

	public void AddStrengthModifier(double newValue)
	{
		_strengthModifier += newValue;
		if (_modifierMin > _strengthModifier) _strengthModifier = _modifierMin;
		if (_modifierMax < _strengthModifier) _strengthModifier = _modifierMax;
	}
	public void AddElementalModifier(double newValue)
	{
		_elementalModifier += newValue;
		if (_modifierMin > _elementalModifier) _elementalModifier = _modifierMin;
		if (_modifierMax < _elementalModifier) _elementalModifier = _modifierMax;
	}
	public void AddDefenseModifier(double newValue)
	{
		_defenseModifier += newValue;
		if (_modifierMin > _defenseModifier) _defenseModifier = _modifierMin;
		if (_modifierMax < _defenseModifier) _defenseModifier = _modifierMax;
	}
	public void AddAgilityModifier(double newValue)
	{
		_agilityModifier += newValue;
		if (_modifierMin > _agilityModifier) _agilityModifier = _modifierMin;
		if (_modifierMax < _agilityModifier) _agilityModifier = _modifierMax;
	}
	public void AddAccuraccyModifier(double newValue)
	{
		_accuraccyModifier += newValue;
		if (_modifierMin > _accuraccyModifier) _accuraccyModifier = _modifierMin;
		if (_modifierMax < _accuraccyModifier) _accuraccyModifier = _modifierMax;
	}
	public void AddCritModifier(double newValue)
	{
		_critModifier += newValue;
		if (_modifierMin > _critModifier) _critModifier = _modifierMin;
		if (_modifierMax < _critModifier) _critModifier = _modifierMax;
	}
	
	public double GetStrengthModifier() { return _strengthModifier; }
	public double GetElementalModifier() { return _elementalModifier; }
	public double GetDefenseModifier() { return _defenseModifier; }
	public double GetAgilityModifier() { return _agilityModifier; }
	public double GetAccuraccyModifier() { return _accuraccyModifier; }
	public double GetCritModifier() { return _critModifier; }

	public void SetCharge(bool enable) { _chargeEnabled = enable; }
	public void SetFocus(bool enable) { _focusEnabled = enable; }
	public bool IsChargeEnabled() { return _chargeEnabled; }
	public bool IsFocusEnabled() { return _focusEnabled; }

	public BattleConsts.AffinityType GetAffinity(BattleConsts.ElementType elementKey) { return _characterAffinity.GetAffinity(elementKey); }

	public Godot.Collections.Array<BaseSkillResource> GetSkills() { return _skills; }
	public Godot.Collections.Array<UseableSkillResource> GetUseableSkills() { return _useableSkills; }
	public Godot.Collections.Array<ActivePassiveSkill> GetPassiveSkills() { return _passiveSkills; }
	public Godot.Collections.Array<FusionSkillResource> GetFusionSkills() { return _fusionSkills; }

	public ActiveStatusCondition GetActiveStatusCondition() { return _activeStatusCondition; }
	public void SetActiveStatusCondition(ActiveStatusCondition newStatusCondition)
	{
		if (_activeStatusCondition != null) RemoveStatusCondition();

		_activeStatusCondition = newStatusCondition;
		_activeStatusCondition.TurnCountChanged += OnStatusConditionTurnCountChanged;
		_activeStatusCondition.TurnCountFinished += OnStatusConditionFinished;

		EmitSignal(SignalName.AddSideEffect, this, (int)_activeStatusCondition.GetTriggerType(), _activeStatusCondition);

		// UI
		_statusIcon.Texture = _activeStatusCondition.GetStatusIcon();
		_statusTurnLabel.Text = _activeStatusCondition.GetTurnCount().ToString();
	}
	public void RemoveStatusCondition()
	{
		// Is there an Active Status Condition?
		if (_activeStatusCondition != null) EmitSignal(SignalName.RemoveSideEffect, this, (int)_activeStatusCondition.GetTriggerType(), _activeStatusCondition);

		_activeStatusCondition = null;
		_statusIcon.Texture = null;
		_statusTurnLabel.Text = "";
	}

	public void AddBuff(ActiveBuff activeBuff)
	{
		// Can't use Contains since ActiveBuff(AttackBuff, 3) and ActiveBuff(AttackBuff, 2) are considered two different values
		bool buffDoesNotExist = true;
		foreach(ActiveBuff buff in _buffs)
		{
			if (buff.GetBuffResource() == activeBuff.GetBuffResource())
			{
				buff.AddTurn(activeBuff.GetTurnCount());
				buffDoesNotExist = false;
				break;
			}
		}

		// Add the buff if it does not already exist
		if (buffDoesNotExist)
		{
			_buffs.Add(activeBuff);
			EmitSignal(SignalName.AddSideEffect, this, (int)activeBuff.GetTriggerType(), activeBuff);

			activeBuff.TurnCountChanged += OnBuffTurnCountChanged;
			activeBuff.BuffFinished += OnBuffFinished;
		}

	}

	/*
		Searches for the specified BuffResource within the ActiveBuffs.
		Primarily used for one-time effects such as Charge, Focus, and Endure.
	*/
	public void SetBuffIsPermanent(BuffResource buffToLookFor, bool isPermanent)
	{
		foreach(ActiveBuff buff in _buffs)
		{
			if (buff.GetBuffResource() == buffToLookFor)
			{
				buff.SetIsPermanent(isPermanent);
				break;
			}
		}
	}

	public void RemoveBuff(ActiveBuff buffToRemove)
	{
		EmitSignal(SignalName.RemoveSideEffect, this, (int)buffToRemove.GetTriggerType(), buffToRemove);
		_buffs.Remove(buffToRemove);
	}

	public void RemoveEachBuff()
	{
		foreach (ActiveBuff activeBuff in _buffs)
		{
			RemoveBuff(activeBuff);
		}
	}

	// Remove all buffs with positive effects
	public void RemoveAllBuffs()
	{
		foreach (ActiveBuff activeBuff in _buffs)
		{
			if (!activeBuff.GetIsDebuff()) RemoveBuff(activeBuff);
		}
	}

	// Remove all buffs with negative effects
	public void RemoveAllDebuffs()
	{
		foreach (ActiveBuff activeBuff in _buffs)
		{
			if (activeBuff.GetIsDebuff()) RemoveBuff(activeBuff);
		}
	}

	public UseableActionResource GetQueueAction() { return _queueAction; }
	public void SetQueueAction(UseableActionResource queueAction) { _queueAction = queueAction; }
	public void RemoveQueueAction() { _queueAction = null; }

	public int GetCounterDamage() { return _counterDamage; }
	public void ResetCounterDamage() { _counterDamage = 0; }

	public void AddTempo(double newTempo) { Tempo += newTempo; }
	public void SetTempo(double newTempo) { Tempo = newTempo; }

	public void AddStun(double newStun) { Stun += newStun; }
	public void SetStun(double newStun) { Stun = newStun; }

	public bool GetIsPlayer() { return _isPlayer; }
	public Texture2D GetBattleIcon() { return _battleIcon; }

	public void ResetReadiness() { 
		Readiness = 0.0; 
		if (_characterStats.GetCurHP() > 0)
		{
			IsActive = true;
		}
	}

	#endregion

	#region Signals
	private void OnStatsHPDepleted()
	{
		GD.Print(_actorName+" - is Dead!");
		IsActive = false;
		IsTargetable = false;

		// Variable resets
		Readiness = 0.0;
		Tempo = 1.0;
		Stun = 0.0;
		RemoveStatusCondition();
		RemoveEachBuff();
		RemoveQueueAction();

		EmitSignal(SignalName.HPDepleted);
	}

	private void OnActorRevived()
	{
		IsActive = true;
		IsTargetable = true;
		EmitSignal(SignalName.HPDepleted);
	}

	// TEMP UI CODE
	private void OnStatusConditionTurnCountChanged(int turnCount)
	{
		_statusTurnLabel.Text = turnCount.ToString();
	}

	private void OnStatusConditionFinished()
	{
		RemoveStatusCondition();
	}

	private void OnBuffTurnCountChanged(int turnCount)
	{
	}

	private void OnBuffFinished(ActiveBuff buff)
	{
		RemoveBuff(buff);
	}

	#endregion
}
