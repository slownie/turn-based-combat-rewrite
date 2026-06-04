using Godot;
using System;

[GlobalClass]
public partial class SetIndestructableEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetIndestructable(target, enable);
    }
}
