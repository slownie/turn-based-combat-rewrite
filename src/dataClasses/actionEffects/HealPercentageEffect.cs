using Godot;
using System;

[GlobalClass]
public partial class HealPercentageEffect : ActionEffectResource
{
    // USER/TARGET STATS ARE NOT FACTORED INTO THE CALCULATION YET
    [Export] int healPercentage = 25;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        int calculatedHeal = Mathf.FloorToInt(target.GetMaxHP() * (0.01 * healPercentage));
        actorController.TakeHeal(target, calculatedHeal);
    }
}
