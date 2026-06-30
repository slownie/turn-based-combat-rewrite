using Godot;
using System;

[GlobalClass]
public partial class WeaponItemResource : EquipmentItemResource
{
	public enum WeaponType
	{
		Gauntlets,
		Staff,
		Hammer
	}
	[Export] WeaponType _weaponType;

	public WeaponType GetWeaponType() { return _weaponType; }
}
