using Godot;
using System;

[GlobalClass]
public partial class CounterAttackEffect : ActionEffectResource
{
    [Export] int baseDamage = 10;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.CounterAttack(user, -baseDamage);
    }
}
