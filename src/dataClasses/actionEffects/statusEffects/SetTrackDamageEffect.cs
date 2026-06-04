using Godot;
using System;

[GlobalClass]
public partial class SetTrackDamageEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetTrackDamage(target, enable);
    }
}
