using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ActorController : Node2D
{

	RandomNumberGenerator _rng;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Godot.Collections.Array<BattleActor> GetLiveActors(Godot.Collections.Array<BattleActor> battleActors)
	{
		Godot.Collections.Array<BattleActor> actors = [];
		IEnumerable<BattleActor> aliveQuery = battleActors.Where(battleActor => battleActor.GetCurHP() > 0);
		foreach (BattleActor actor in aliveQuery) actors.Add(actor);

		return actors;
	}
}
