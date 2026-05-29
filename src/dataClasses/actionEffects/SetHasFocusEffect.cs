using Godot;
using System;

[GlobalClass]
public partial class SetHasFocusEffect : ActionEffectResource
{
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetFocus(target, enable);
    }
}
