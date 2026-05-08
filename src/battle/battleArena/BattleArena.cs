using Godot;
using System;

public partial class BattleArena : Control
{
	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	ActorController _actorController;

	Godot.Collections.Array _partyMembers;
	Godot.Collections.Array<EnemyResource> _enemies;


	public void SetActorData(Godot.Collections.Array<ActivePartyMember> partyMembers, Godot.Collections.Array<EnemyResource> enemies)
	{
		enemies = _enemies;
	}


	public override void _Ready()
	{
		_actorController = GetNode<ActorController>("ActorController");
	}
}
