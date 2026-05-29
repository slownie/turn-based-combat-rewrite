using Godot;
using System;

[GlobalClass]
public partial class SetHasChargeEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetCharge(target, enable);
    }
}