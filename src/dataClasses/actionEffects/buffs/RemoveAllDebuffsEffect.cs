using Godot;
using System;

[GlobalClass]
public partial class RemoveAllDebuffsEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.RemoveAllDebuffs(target);
    }
}
