using Godot;
using System;

[GlobalClass]
public partial class BuffResource : Resource
{
    [Export] string buffName = "";
    [Export] Texture2D icon;

    [Export] PassiveActionResource passiveActionResource;

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }

    public Texture2D GetIcon() { return icon; }
}
