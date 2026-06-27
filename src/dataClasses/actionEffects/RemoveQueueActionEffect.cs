using Godot;
using System;

[GlobalClass]
public partial class RemoveQueueActionEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.RemoveQueueAction(target);
    }
}