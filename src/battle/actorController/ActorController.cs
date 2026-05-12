using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ActorController : Node2D
{

	RandomNumberGenerator _rng;

	public void AddActorCurHP(BattleActor target, int amount) { target.AddCurHP(amount); }
	public void SetActorCurHP(BattleActor target, int amount) { target.SetCurHP(amount); }
	public void AddActorCurMP(BattleActor target, int amount) { target.AddCurMP(amount); }
	public void SetActorCurMP(BattleActor target, int amount) { target.SetCurMP(amount); }

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
}
