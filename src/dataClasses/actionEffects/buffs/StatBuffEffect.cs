using Godot;
using System;

[GlobalClass]
public partial class StatBuffEffect : ActionEffectResource
{
    [Export] BattleConsts.StatBuffType statBuffType;
    [Export] double level;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        switch (statBuffType)
        {
            case BattleConsts.StatBuffType.Strength:
            {
                break;
            }
        }
    }
}
