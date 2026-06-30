using Godot;
using System;

[GlobalClass]
public partial class EquipmentItemResource : BaseItemResource
{
	public enum EquipmentType
	{
		Weapon,
		Armor,
		Accessory
	}

	[Export] BaseStats baseStats;
	[Export] PassiveActionResource passiveActionResource;

	public BaseStats GetBaseStats() { return baseStats; }

	public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
	public bool GetRunOnce() { return passiveActionResource.GetRunOnce(); }
}
