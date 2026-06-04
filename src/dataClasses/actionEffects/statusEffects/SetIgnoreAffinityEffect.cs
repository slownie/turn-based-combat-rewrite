using Godot;
using System;

[GlobalClass]
public partial class SetIgnoreAffinityEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetIgnoreAffinity(target, enable);
    }
}
