using Godot;
using System;

public partial class ActivePassiveSkill : ActivePassiveEffect
{
    PassiveSkillResource _passiveSkillResource;

    public ActivePassiveSkill(PassiveSkillResource passiveSkillResource) : base(
        passiveSkillResource.GetPassiveActionResource().GetTriggerActions(),
        passiveSkillResource.GetPassiveActionResource().GetStartupActions(),
        passiveSkillResource.GetPassiveActionResource().GetCleanupActions()
    )
    {
        _passiveSkillResource = passiveSkillResource;
        _triggerType = _passiveSkillResource.GetPassiveActionResource().GetTriggerType();
        _runOnce = _passiveSkillResource.GetPassiveActionResource().GetRunOnce();
        _oneShot = _passiveSkillResource.GetPassiveActionResource().GetOneShot();
    }
}
