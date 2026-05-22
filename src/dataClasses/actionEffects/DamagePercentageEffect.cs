using Godot;
using System;

[GlobalClass]
public partial class DamagePercentageEffect : ActionEffectResource
{
    [Export] int damagePercentage = 25;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        int calculatedDamage = Mathf.FloorToInt(target.GetMaxHP() * (0.01 * damagePercentage));
        actorController.TakeDamage(target, -calculatedDamage, false);
    }

}
