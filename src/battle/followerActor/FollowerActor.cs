using Godot;
using System;

/*
    Serve as a psuedo-actor for enemies.

*/
public partial class FollowerActor : Node2D
{
    /*
        Argument could be made for composition here, I'll do it later
    */
    [Signal] public delegate void ReadinessChangedEventHandler(double readiness);
    [Signal] public delegate void ReadyToActEventHandler(FollowerActor followerActor);
    
	const double _modifierMin = 0.25;
	const double _modifierMax = 2;

    BattleActor _leaderActor;

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
	double _battleSpeed = 10;

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
    #endregion

    public void Setup(BattleActor leaderActor)
    {
        _leaderActor = leaderActor;
		_leaderActor.HPDepleted += OnStatsHPDepleted;
    }

	public override void _Process(double delta)
	{
		if (Tempo != 1.0)
		{
			// Tempo
			if (Tempo < 1.01) Tempo += (_tempoRate * TimeScale * delta);
			if (1.01 < Tempo) Tempo -= (_tempoRate * TimeScale * delta);
		}

		if (0.0 < Stun)
		{
			// Stun
			Stun -= _battleSpeed * TimeScale * delta;
		} else {
			// Readiness
			Readiness += _battleSpeed * (_leaderActor.GetAgility() * _leaderActor.GetAgilityModifier() * Tempo) * TimeScale * delta;
		}
	}

    public BattleActor GetLeaderActor() { return _leaderActor; }

	public void AddReadiness(double newReadiness) { Readiness += newReadiness; }
    public void ResetReadiness()
    {
        Readiness = 0.0; 
		if (_leaderActor.GetCurHP() > 0)
		{
			IsActive = true;
		}
    }

	private void OnStatsHPDepleted()
	{
		IsActive = false;

		// Variable resets
		Readiness = 0.0;

		EmitSignal(SignalName.ReadinessChanged, Readiness);
	}
}