using Godot;
using System;

[GlobalClass]
public partial class HealEffect : ActionEffectResource
{
    // USER/TARGET STATS ARE NOT FACTORED INTO THE CALCULATION YET
    [Export] int baseHeal = 25;
    [Export] bool useUserStats = false;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.TakeHeal(target, baseHeal);
    }
}
