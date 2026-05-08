using Godot;
using System;

public partial class BattleArena : Control
{
	[Export] PackedScene battleActorScene;

	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	ActorController _actorController;

	Godot.Collections.Array<BattleActor> _actors = [];

	public void SetupActors(Godot.Collections.Array<ActivePartyMember> partyMembers, Godot.Collections.Array<EnemyResource> enemies)
	{
		
		foreach (ActivePartyMember activePartyMember in partyMembers)
		{
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
		}
	}


	public override void _Ready()
	{
		_actorController = GetNode<ActorController>("ActorController");
	}
}
