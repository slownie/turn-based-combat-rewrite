using Godot;
using System;

public partial class BattleActor : Node2D
{
	[Signal] public delegate void HPDepletedEventHandler();


	[Signal] public delegate void ReadinessChangedEventHandler(double readiness);
	[Signal] public delegate void ReadyToActEventHandler();

	string _name = "Placeholder";
	CharacterStats _characterStats;

	bool _isPlayer = true;


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
				EmitSignal(SignalName.ReadyToAct);
				SetProcess(false);
			}
		}
	}
	#endregion

	// If 'false' this actor cannot be targeted by any action
	public bool IsTargetable {get; set;} = true;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IsActive = false;
	}

	public override void _Process(double delta)
	{
		Readiness += 10 * _characterStats.GetAgility() * TimeScale * delta;
	}

	public void Setup(string actorName, CharacterStats characterStats)
	{
		_name = actorName;
		_characterStats = characterStats;
	}

	private void OnStatsHPDepleted()
	{
		Readiness = 0.0;
		IsActive = false;
		IsTargetable = false;
		EmitSignal(SignalName.HPDepleted);
	}
}
