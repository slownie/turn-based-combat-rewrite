using Godot;
using System;

[GlobalClass]
public partial class RejuvenatePercentageEffect : ActionEffectResource
{
    // USER/TARGET STATS ARE NOT FACTORED INTO THE CALCULATION YET
    [Export] int rejuventagePercentage = 25;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        int calculatedRejuvenate = Mathf.FloorToInt(target.GetMaxMP() * (0.01 * rejuventagePercentage));
        actorController.TakeRejuvenate(target, calculatedRejuvenate);
    }
}
