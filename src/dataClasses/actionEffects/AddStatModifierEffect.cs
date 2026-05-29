using Godot;
using System;

[GlobalClass]
public partial class AddStatModifierEffect : ActionEffectResource
{
    [Export] BuffResource buffResource;
    [Export] int turnCount = 3;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
    }
}
