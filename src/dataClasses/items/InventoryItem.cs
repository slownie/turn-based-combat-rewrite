using Godot;
using System;

public partial class InventoryItem : GodotObject
{
	BaseItemResource _itemResource;
	int _quantity;

	public InventoryItem() : this(null, -1) {}
	public InventoryItem(BaseItemResource itemResource, int quantity)
	{
		_itemResource = itemResource;
		_quantity = quantity;
	}

	public BaseItemResource GetItemResource() { return _itemResource; }
	public int GetItemQuantity() { return _quantity; }
}
