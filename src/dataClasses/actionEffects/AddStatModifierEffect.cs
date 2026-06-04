using Godot;
using System;

[GlobalClass]
public partial class AddStatModifierEffect : ActionEffectResource
{
    [Export] BattleConsts.StatBuffType statBuff;
    [Export] double statLevel;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.AddStatModifier(target, statBuff, statLevel);
    }
}
