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

	// If we were making something fancier, these would be booleans
	public enum EquipRestriction
	{
		Any,
		Terra,
		Alpheus,
		Suza
	}

	[Export] EquipmentType equipmentType;
	[Export] EquipRestriction equipRestriction;
	[Export] BaseStats baseStats;
	[Export] PassiveActionResource passiveActionResource;

	public BaseStats GetBaseStats() { return baseStats; }

	public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
	public bool GetRunOnce() { return passiveActionResource.GetRunOnce(); }

	public EquipmentType GetEquipmentType() { return equipmentType; }
	public EquipRestriction GetEquipRestriction() { return equipRestriction; }
}
