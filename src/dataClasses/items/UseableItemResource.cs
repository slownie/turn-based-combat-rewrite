using Godot;
using System;

[GlobalClass]
public partial class UseableItemResource : BaseItemResource
{
	[Export] UseableActionResource useableActionResource;

	public UseableActionResource GetUseableActionResource() { return useableActionResource; }
}
