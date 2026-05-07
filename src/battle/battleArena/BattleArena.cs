using Godot;
using System;

public partial class BattleArena : Control
{
	[Signal] public delegate void BattleFinishedEventHandler(BattleOutcome outcome);

	public enum BattleOutcome
	{
		Victory,
		Defeat,
		Escape,
		Interrupt
	}


	public override void _Ready()
	{
	}

	public void BindServices()
	{
		
	}

	public void Setup()
	{
		// Create Actors
	}
}
