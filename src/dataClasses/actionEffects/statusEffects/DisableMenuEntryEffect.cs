using Godot;
using System;

[GlobalClass]
public partial class DisableMenuEntryEffect : ActionEffectResource
{
    [Export] BattleConsts.MenuEntryType menuEntryTypeToDisable = BattleConsts.MenuEntryType.SkillPhysical;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
       
    }
}
