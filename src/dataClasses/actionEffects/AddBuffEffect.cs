using Godot;
using System;

[GlobalClass]
public partial class AddBuffEffect : ActionEffectResource
{
    [Export] BuffResource buffResource;
    [Export] int turnCount = 3;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.AddBuff(target, buffResource, turnCount);
    }
}
