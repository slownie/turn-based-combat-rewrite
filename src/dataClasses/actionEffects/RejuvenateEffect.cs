using Godot;
using System;

[GlobalClass]
public partial class RejuvenateEffect : ActionEffectResource
{
    // USER/TARGET STATS ARE NOT FACTORED INTO THE CALCULATION YET
    [Export] int baseRejuvenate = 25;
    [Export] bool useUserStats = false;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.TakeRejuvenate(target, baseRejuvenate);
    }
}
