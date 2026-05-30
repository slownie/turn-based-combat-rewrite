using Godot;
using System;

[GlobalClass]
public partial class SetStunEffect : ActionEffectResource
{
    [Export] double stunDuration = 0.0;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetStun(target, stunDuration);
    }
}
