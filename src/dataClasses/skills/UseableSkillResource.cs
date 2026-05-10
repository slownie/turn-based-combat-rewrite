using Godot;
using System;

[GlobalClass]
public partial class UseableSkillResource : BaseSkillResource
{
	public enum SkillCostType
	{
		HP,
		MP
	}

	[Export] SkillCostType skillCostType = SkillCostType.MP;
	[Export] int skillCostAmount = 0;
	[Export] Godot.Collections.Array<ActionEffectResource> actions = [];
}
