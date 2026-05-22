using Godot;
using System;

public partial class BattleActor : Node2D
{
	// Stat Signals
	[Signal] public delegate void HPChangedEventHandler(int newHP);
	[Signal] public delegate void HPDepletedEventHandler();

	[Signal] public delegate void MPChangedEventHandler(int newMP);

	[Signal] public delegate void ActorTurnStartEventHandler();

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

	string _actorName = "Placeholder";
	CharacterStats _characterStats;
	Godot.Collections.Array<BaseSkillResource> _skills = [];

	ActiveStatusCondition _activeStatusCondition = null;

	bool _isPlayer = true;

	AnimatedSprite2D _sprite;
	Texture2D _battleIcon;

	Label _curHPLabel;
	Label _maxHPLabel;

	Label _curMPLabel;
	Label _maxMPLabel;

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

		_curHPLabel = GetNode<Label>("TestUI/HPContainer/CurHP");
		_maxHPLabel = GetNode<Label>("TestUI/HPContainer/MaxHP");
		_curMPLabel = GetNode<Label>("TestUI/MPContainer/CurMP");
		_maxMPLabel = GetNode<Label>("TestUI/MPContainer/MaxMP");

		IsActive = false;
	}

	public override void _Process(double delta)
	{
		Readiness += 10 * _characterStats.GetAgility() * TimeScale * delta;
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

		_skills = skills;

		_activeStatusCondition = null;

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

	public void TurnStart()
	{
		EmitSignal(SignalName.ActorTurnStart);
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

	public Godot.Collections.Array<BaseSkillResource> GetSkills() { return _skills; }

	public ActiveStatusCondition GetActiveStatusCondition() { return _activeStatusCondition; }
	public void SetActiveStatusCondition(ActiveStatusCondition newStatusCondition)
	{
		_activeStatusCondition = newStatusCondition;
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
	#endregion
}
