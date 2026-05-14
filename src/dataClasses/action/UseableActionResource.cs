using Godot;
using System;

[GlobalClass]
public partial class UseableActionResource : Resource
{
	[Export] Godot.Collections.Array<ActionEffectResource> actions = [];

	[Export] TargetingSettings targetingSettings;

	public TargetingSettings GetTargetingSettings() { return targetingSettings; }
}
