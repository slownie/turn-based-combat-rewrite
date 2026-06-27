using Godot;
using System;

[GlobalClass]
public partial class EscapeEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
		if (GD.Randi() % 99 + user.GetAgility() > GD.Randi() % 99)
		{
			actorController.DidEscape();
		}
    }
}
