using Godot;
using System;

[GlobalClass]
public partial class SetIgnoreSkillCostEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetIgnoreSkillCosts(target, enable);
    }
}
