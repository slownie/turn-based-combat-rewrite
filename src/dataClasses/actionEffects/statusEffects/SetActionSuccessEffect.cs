using Godot;
using System;

[GlobalClass]
public partial class SetActionSuccessEffect : ActionEffectResource
{
    [Export] bool enable = true;
    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetSkillSuccessGuarnatee(target, enable);
    }
}
