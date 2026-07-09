using Godot;
using System;

public partial class InventoryController : Node2D
{
	const int ItemMax = 99;

	public void AddItemQuantity(Godot.Collections.Array<InventoryItem> inventory, int itemIndex, int quantity)
	{
		// Does the index exist in the inventory?
		if (0 < inventory.Count && itemIndex <= inventory.Count - 1)
		{
			inventory[itemIndex].AddItemQuantity(quantity);

			// Clamp
			if (ItemMax < inventory[itemIndex].GetItemQuantity()) inventory[itemIndex].SetItemQuantity(ItemMax);
			if (inventory[itemIndex].GetItemQuantity() <= 0) inventory.RemoveAt(itemIndex);

			GD.Print(inventory[itemIndex].GetItemName()+" "+inventory[itemIndex].GetItemQuantity());
		} else {
			GD.Print("Inventory AddItemQuantity - Specified index not found");
		}
	}

	// Equipment
	public void EquipItem(ActivePartyMember partyMember, EquipmentItem equipmentItem)
	{
		if (equipmentItem.GetUser() != null)
		{
			UnequipItem(equipmentItem.GetUser(), equipmentItem.GetEquipmentType());
		}
		partyMember.EquipItem(equipmentItem);
	}

	public void UnequipItem(ActivePartyMember partyMember, EquipmentItemResource.EquipmentType equipmentType)
	{
		partyMember.UnequipItem(equipmentType);
	}
}
