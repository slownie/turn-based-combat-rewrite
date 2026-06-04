using Godot;
using System;

[GlobalClass]
public partial class FusionSkillResource : UseableSkillResource
{
	[Export] int partnerFusionID;

	public int GetFusionID() { return partnerFusionID; }
}
