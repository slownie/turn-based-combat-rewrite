using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillResource : BaseSkillResource
{
    [Export] PassiveActionResource passiveActionResource;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public bool GetRunOnce() { return passiveActionResource.GetRunOnce(); }
    public bool GetOneShot() { return passiveActionResource.GetRunOnce(); }
}
