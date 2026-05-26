using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillResource : BaseSkillResource
{
    [Export] PassiveActionResource passiveActionResource;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
}
