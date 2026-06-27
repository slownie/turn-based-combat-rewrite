using Godot;
using System;

[GlobalClass]
public partial class RemoveEveryBuffEffect : ActionEffectResource
{
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.RemoveEveryBuff(target);
    }
}
