using Godot;
using System;

[GlobalClass]
public partial class UseableItemResource : BaseItemResource
{
	[Export] Godot.Collections.Array<ActionEffectResource> actions = [];
}
