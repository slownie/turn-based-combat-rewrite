using Godot;
using System;

[GlobalClass]
public partial class SetSelectRandomActionEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetSelectRandomAction(target, enable);
    }
}
