using Godot;
using System;

public partial class CombatZone : Area2D
{
	[Export] EnemyEncounterResource _enemyEncounterResource;

	[Signal] public delegate void EncounterStartEventHandler(EnemyEncounterResource enemyEncounterResource);

	CollisionShape2D _collisionShape;
	
	private void OnBodyEntered(Node2D node)
	{
		if (node is OverworldPlayer)
		{
			GD.Print("Combat Start");
			EmitSignal(SignalName.EncounterStart, _enemyEncounterResource);
		}
	}
}
