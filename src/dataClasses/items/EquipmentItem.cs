using Godot;
using System;

[GlobalClass]
public partial class EquipmentItem : InventoryItem
{
	EquipmentItemResource _equipmentItemResource;
	ActivePartyMember _equippedUser = null;

	public EquipmentItem() : this(null) {}
	public EquipmentItem(EquipmentItemResource equipmentItemResource) : base(equipmentItemResource, -1)
	{
		_equipmentItemResource = equipmentItemResource;
	}

	public EquipmentItemResource GetEquipmentItemResource() { return _equipmentItemResource; }
	public BaseStats GetEquipmentBaseStats() { return _equipmentItemResource.GetBaseStats(); }

	public EquipmentItemResource.EquipmentType GetEquipmentType() { return _equipmentItemResource.GetEquipmentType(); }
	public EquipmentItemResource.EquipRestriction GetEquipRestriction() { return _equipmentItemResource.GetEquipRestriction(); }


	public void SetUser(ActivePartyMember user) { _equippedUser = user; }
	public ActivePartyMember GetUser() { return _equippedUser; }
	public void RemoveUser() { _equippedUser = null; }
}
