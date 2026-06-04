using Godot;
using System;

[GlobalClass]
public partial class ForceActionEffect : ActionEffectResource
{
    [Export] UseableActionResource forceAction;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        //actorController.ForceSelectAction(user, target, forceAction);
    }
}
