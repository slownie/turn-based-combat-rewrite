using Godot;
using System;

public partial class ActivePassiveSkill : ActivePassiveEffect
{
    PassiveSkillResource _passiveSkillResource;
    bool _isActive = true;

    public ActivePassiveSkill(PassiveSkillResource passiveSkillResource) : base(
        passiveSkillResource.GetPassiveActionResource().GetTriggerActions(),
        passiveSkillResource.GetPassiveActionResource().GetStartupActions(),
        passiveSkillResource.GetPassiveActionResource().GetCleanupActions()
    )
    {
        _passiveSkillResource = passiveSkillResource;
        _triggerType = _passiveSkillResource.GetPassiveActionResource().GetTriggerType();
        _runOnce = _passiveSkillResource.GetPassiveActionResource().GetRunOnce();
    }

    public bool GetIsActive() { return _isActive; }
    public void SetIsActive(bool value) { _isActive = value; }    
}
