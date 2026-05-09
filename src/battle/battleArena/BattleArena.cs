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
		// Party Members		
		for (int i=0; i < partyMembers.Count; i++)
		{
			ActivePartyMember activePartyMember = partyMembers[i];
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
			
			newActor.Setup(
				250+(i % 3*10)+(i / 3*25),
				68+(i % 3*20),
				activePartyMember.GetPartyMemberName(), 
				activePartyMember.GetCharacterStats(),
				activePartyMember.GetSpriteFrames(),
				true
			);

			_actors.Add(newActor);
		}

		// Enemies
		for (int i=0; i < enemies.Count; i++)
		{
			EnemyResource enemy = enemies[i];
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
			
			newActor.Setup(
				70+(i % 2*35)+(i / 3*40),
				58+(i % 3*30),
				enemy.GetEnemyName(), 
				enemy.GetCharacterStats(),
				enemy.GetSpriteFrames(),
				false
			);

			_actors.Add(newActor);
		}	
	}


	public override void _Ready()
	{
		_actorController = GetNode<ActorController>("ActorController");
	}

	public void StartBattle()
	{
	}
}
