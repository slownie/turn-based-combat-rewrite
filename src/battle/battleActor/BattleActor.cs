using Godot;
using System;

public partial class BattleActor : Node2D
{
	[Signal] public delegate void HPDepletedEventHandler();


	[Signal] public delegate void ReadinessChangedEventHandler(double readiness);
	[Signal] public delegate void ReadyToActEventHandler(BattleActor battleActor);

	string _name = "Placeholder";
	CharacterStats _characterStats;

	bool _isPlayer = true;

	AnimatedSprite2D _sprite;
	Texture2D _battleIcon;

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

	#endregion
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
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
		SpriteFrames spriteFrames, 
		Texture2D battleIcon,
		bool isPlayer
	)
	{
		Vector2 newPosition = new Vector2(x, y);
		Position = newPosition;

		_name = actorName;
		_characterStats = characterStats;

		GD.Print(_characterStats.GetMaxHP());

		_sprite.SpriteFrames = spriteFrames;
		_sprite.Play("default");

		_battleIcon = battleIcon;

		_isPlayer = isPlayer;
		if (_isPlayer) _sprite.FlipH = true;
	}

	#region Getters

	public bool GetIsPlayer() { return _isPlayer; }
	public Texture2D GetBattleIcon() { return _battleIcon; }

	#endregion

	#region Signals
	private void OnStatsHPDepleted()
	{
		Readiness = 0.0;
		IsActive = false;
		IsTargetable = false;
		EmitSignal(SignalName.HPDepleted);
	}
	#endregion
}
