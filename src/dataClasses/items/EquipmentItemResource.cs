using Godot;
using System;

[GlobalClass]
public partial class EquipmentItemResource : BaseItemResource
{
	public enum EquipmentType
	{
		Armor,
		Accessory
	}

	[Export] EquipmentType equipmentType;
	[Export] BaseStats baseStats;
	[Export] PassiveActionResource passiveActionResource;

	public BaseStats GetBaseStats() { return baseStats; }

	public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
	public bool GetRunOnce() { return passiveActionResource.GetRunOnce(); }

	public EquipmentType GetEquipmentType() { return equipmentType; }
}
