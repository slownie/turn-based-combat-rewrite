using Godot;
using System;

public partial class ActivePassiveSkill : GodotObject
{
    

    PassiveSkillResource _passiveSkillResource;
    BattleConsts.TriggerType _triggerType;

    public ActivePassiveSkill() : this(null) {}
    public ActivePassiveSkill(PassiveSkillResource passiveSkillResource)
    {
        _triggerType = passiveSkillResource.GetPassiveActionResource().GetTriggerType();
    }

    public Godot.Collections.Array<ActionEffectResource> GetTriggerActions() { return _passiveSkillResource.GetPassiveActionResource().GetTriggerActions(); }
    public Godot.Collections.Array<ActionEffectResource> GetCleanupActions() { return _passiveSkillResource.GetPassiveActionResource().GetCleanupActions(); }

    public BattleConsts.TriggerType GetTriggerType() { return _triggerType; }
}
