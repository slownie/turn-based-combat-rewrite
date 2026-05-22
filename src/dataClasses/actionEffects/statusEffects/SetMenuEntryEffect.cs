using Godot;
using System;

[GlobalClass]
public partial class SetMenuEntryEffect : ActionEffectResource
{
    [Export] BattleConsts.MenuEntryType menuEntryType = BattleConsts.MenuEntryType.SkillPhysical;
    [Export] bool enable = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        actorController.SetMenuEntry(user, menuEntryType, enable);
    }
}
