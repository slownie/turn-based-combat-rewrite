using Godot;
using System;

[GlobalClass]
public partial class EquipmentItem : InventoryItem
{
	EquipmentItemResource _equipmentItemResource;
	ActivePartyMember _equippedUser = null;

	public EquipmentItem() : this(null) {}
	public EquipmentItem(EquipmentItemResource equipmentItemResource)
	{
		_equipmentItemResource = equipmentItemResource;
	}

	public EquipmentItemResource GetEquipmentItemResource() { return _equipmentItemResource; }
	public BaseStats GetEquipmentBaseStats() { return _equipmentItemResource.GetBaseStats(); }

	public void SetUser(ActivePartyMember user) { _equippedUser = user; }
	public void RemoveUser() { _equippedUser = null; }
}
