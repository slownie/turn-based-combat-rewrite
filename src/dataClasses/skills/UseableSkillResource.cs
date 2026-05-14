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
	[Export] UseableActionResource useableActionResource;

	public UseableActionResource GetUseableActionResource() { return useableActionResource; }
	public SkillCostType GetSkillCostType() { return skillCostType; }
	public int GetSkillCostAmount() { return skillCostAmount; }
}
