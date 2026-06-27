using Godot;
using System;

[GlobalClass]
public partial class RemoveStatusConditionEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.RemoveStatusCondition(target);
    }
}
