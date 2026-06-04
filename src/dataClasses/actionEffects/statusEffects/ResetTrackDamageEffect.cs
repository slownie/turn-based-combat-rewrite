using Godot;
using System;

[GlobalClass]
public partial class ResetTrackDamageEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.ResetCounterDamage(target);
    }
}
