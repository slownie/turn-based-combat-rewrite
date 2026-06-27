using Godot;
using System;

[GlobalClass]
public partial class QueueActionEffect : ActionEffectResource
{
    [Export] UseableActionResource useableActionResource;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetQueueAction(target, useableActionResource);
    }
}
