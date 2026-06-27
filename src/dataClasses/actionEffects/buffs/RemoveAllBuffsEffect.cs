using Godot;
using System;

[GlobalClass]
public partial class RemoveAllBuffsEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.RemoveAllBuffs(target);
    }
}
