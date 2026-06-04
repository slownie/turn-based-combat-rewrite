using Godot;
using System;

[GlobalClass]
public partial class AddTempoEffect : ActionEffectResource
{
    [Export] double tempo = 0.0;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.AddTempo(setForUser ? user : target, tempo);
    }
}
