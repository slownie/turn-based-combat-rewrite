using Godot;
using System;

[GlobalClass]
public partial class AddBuffEffect : ActionEffectResource
{
    [Export] BuffResource buffResource;
    [Export] int turnCount = 3;
    [Export] bool isPermanent = false;
    

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.AddBuff(target, buffResource, turnCount, isPermanent);
    }
}
