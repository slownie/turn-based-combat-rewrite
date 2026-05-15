using Godot;
using System;

[GlobalClass]
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

	public Texture2D GetIcon() { return _itemResource.GetIcon(); }
	public string GetItemName() { return _itemResource.GetItemName(); }
	public int GetItemQuantity() { return _quantity; }

	public void AddItemQuantity(int itemQuantity) { _quantity += itemQuantity; } 
	public void SetItemQuantity(int itemQuantity) { _quantity = itemQuantity; }
}
