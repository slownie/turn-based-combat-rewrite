using Godot;
using System;

[GlobalClass]
public partial class PassiveSkillResource : BaseSkillResource
{
    [Export] PassiveActionResource passiveActionResource;
    [Export] bool runOnce = false;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public bool GetRunOnce() { return runOnce; }
}
