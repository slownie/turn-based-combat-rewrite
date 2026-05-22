using Godot;
using System;

[GlobalClass]
public partial class AddStatusConditionEffect : ActionEffectResource
{
    [Export] StatusConditionResource statusCondition;
    [Export] int turnCount = 3;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetStatusCondition(target, statusCondition, turnCount);
    }
}
