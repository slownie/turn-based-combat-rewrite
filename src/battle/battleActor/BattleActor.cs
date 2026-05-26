using Godot;
using System;

public partial class BattleActor : Node2D
{
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
	[Signal] public delegate void AddSideEffectEventHandler(BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect);
	[Signal] public delegate void RemoveSideEffectEventHandler(BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect);


	string _actorName = "Placeholder";
	CharacterStats _characterStats;

	// EQUIPMENT HERE

	Godot.Collections.Array<BaseSkillResource> _skills = [];
	Godot.Collections.Array<UseableSkillResource> _useableSkills = [];
	Godot.Collections.Array<PassiveSkillResource> _passiveSkills = [];

	ActiveStatusCondition _activeStatusCondition = null;

	Godot.Collections.Array _buffs = [];

	double _strengthModifier = 1;
	double _elementalModifier = 1;
	double _defenseModifier = 1;
	double _agilityModifier = 1;

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

	Godot.Collections.Dictionary<BattleConsts.ElementType, double> _affinity = new Godot.Collections.Dictionary<BattleConsts.ElementType, double>()
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

	const double _modifierMin = 0.25;
	const double _modifierMax = 2;

	bool _chargeEnabled = false;
	bool _focusEnabled = false;


	bool _isPlayer = true;

	AnimatedSprite2D _sprite;
	Texture2D _battleIcon;

	// Test UI
	TextureRect _statusIcon;
	Label _statusTurnLabel;

	Label _curHPLabel;
	Label _maxHPLabel;

	Label _curMPLabel;
	Label _maxMPLabel;

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

	// When this value reaches '100.0', the battler is ready to take their turn
	double readiness = 0.0;
	public double Readiness
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

	// If 'false' this actor cannot be targeted by any action
	public bool IsTargetable {get; set;} = true;


	// Status Effect Parameters

	/*
		When true, the user will select a random action.
		Primarily used by the Confuse status effect.
	*/
	public bool SelectRandomAction {get; set; } = false; 

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

	#endregion
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");

		_statusIcon = GetNode<TextureRect>("TestUI/StatusContainer/StatusIcon");
		_statusTurnLabel = GetNode<Label>("TestUI/StatusContainer/StatusTurn");

		_curHPLabel = GetNode<Label>("TestUI/StatContainer/HPContainer/CurHP");
		_maxHPLabel = GetNode<Label>("TestUI/StatContainer/HPContainer/MaxHP");
		_curMPLabel = GetNode<Label>("TestUI/StatContainer/MPContainer/CurMP");
		_maxMPLabel = GetNode<Label>("TestUI/StatContainer/MPContainer/MaxMP");

		IsActive = false;
	}

	public override void _Process(double delta)
	{
		Readiness += 10 * (_characterStats.GetAgility() * _agilityModifier) * TimeScale * delta;
	}

	public void Setup(
		int x, 
		int y, 
		string actorName, 
		CharacterStats characterStats, 
		Godot.Collections.Array<BaseSkillResource> skills,
		SpriteFrames spriteFrames, 
		Texture2D battleIcon,
		bool isPlayer
	)
	{
		Vector2 newPosition = new Vector2(x, y);
		Position = newPosition;

		_actorName = actorName;
		
		_characterStats = characterStats;
		_characterStats.HPDepleted += OnStatsHPDepleted;

		_activeStatusCondition = null;

		_skills = skills;

		// Skill sorting
		foreach (BaseSkillResource skill in _skills)
		{
			if (skill is UseableSkillResource)
			{
				_useableSkills.Add(skill as UseableSkillResource);
			}

			if (skill is PassiveSkillResource)
			{
				PassiveSkillResource _passiveSkill = skill as PassiveSkillResource;
				_passiveSkills.Add(_passiveSkill);
			}
		}

		

		_curHPLabel.Text = _characterStats.GetCurHP().ToString();
		_maxHPLabel.Text = _characterStats.GetMaxHP().ToString();
		_curMPLabel.Text = _characterStats.GetCurMP().ToString();
		_maxMPLabel.Text = _characterStats.GetMaxMP().ToString();

		_sprite.SpriteFrames = spriteFrames;
		_sprite.Play("default");

		_battleIcon = battleIcon;

		_isPlayer = isPlayer;
		if (_isPlayer) _sprite.FlipH = true;

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

	#region Getters

	public string GetActorName() { return _actorName; }

	// Stats
	public int GetCurHP() { return _characterStats.GetCurHP(); }

	public int GetMaxHP() { return _characterStats.GetMaxHP(); }

	public int GetCurMP() { return _characterStats.GetCurMP(); }
	public int GetMaxMP() { return _characterStats.GetMaxMP(); }

	public void AddCurHP(int amount) {
		// This should also prevent OnDeath effects from activating since 
		// HP never reaches 0 which would cause those effects to activate.
		if (IsImmortal)
		{
			// Would this kill the actor?
			if (_characterStats.GetCurHP() <= amount)
			{
				SetCurHP(1);
			}
		}

		_characterStats.AddCurHP(amount); 
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
	
	public double GetStrengthModifier() { return _strengthModifier; }
	public double GetElementalModifier() { return _elementalModifier; }
	public double GetDefenseModifier() { return _defenseModifier; }
	public double GetAgilityModifier() { return _agilityModifier; }

	public void SetCharge(bool enable) { _chargeEnabled = enable; }
	public void SetFocus(bool enable) { _focusEnabled = enable; }
	public bool IsChargeEnabled() { return _chargeEnabled; }
	public bool IsFocusEnabled() { return _focusEnabled; }

	public Godot.Collections.Array<BaseSkillResource> GetSkills() { return _skills; }
	public Godot.Collections.Array<UseableSkillResource> GetUseableSkills() { return _useableSkills; }
	public Godot.Collections.Array<PassiveSkillResource> GetPassiveSkills() { return _passiveSkills; }

	public ActiveStatusCondition GetActiveStatusCondition() { return _activeStatusCondition; }
	public void SetActiveStatusCondition(ActiveStatusCondition newStatusCondition)
	{
		_activeStatusCondition = newStatusCondition;
		_activeStatusCondition.TurnCountChanged += OnStatusConditionTurnCountChanged;
		_activeStatusCondition.TurnCountFinished += OnStatusConditionFinished;

		EmitSignal(SignalName.AddSideEffect, (int)_activeStatusCondition.GetTriggerType(), _activeStatusCondition);

		// UI
		_statusIcon.Texture = _activeStatusCondition.GetStatusIcon();
		_statusTurnLabel.Text = _activeStatusCondition.GetTurnCount().ToString();
	}

	public void RemoveStatusCondition()
	{
		EmitSignal(SignalName.RemoveSideEffect, (int)_activeStatusCondition.GetTriggerType(), _activeStatusCondition);

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
				buff.AddTurnCount(activeBuff.GetTurnCount());
				buffDoesNotExist = false;
				break;
			}
		}

		// Add the buff if it does not already exist
		if (buffDoesNotExist)
		{
			_buffs.Add(activeBuff);
			activeBuff.TurnCountChanged += OnBuffTurnCountChanged;
			activeBuff.BuffFinished += OnBuffFinished;
		}
	}

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
		Readiness = 0.0;
		IsActive = false;
		IsTargetable = false;
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
		_buffs.Remove(buff);
	}

	#endregion
}
