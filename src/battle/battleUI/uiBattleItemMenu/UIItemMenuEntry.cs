using Godot;
using System;

public partial class UIItemMenuEntry : UIBattleMenuEntryBase
{
    Label _quantityText;

    public override void _Ready()
    {
        base._Ready();
        _quantityText = GetNode<Label>("HBoxContainer/Quantity");
    }

    public void Setup(InventoryItem inventoryItem)
    {
        _icon.Texture = inventoryItem.GetIcon();
        _nameText.Text = inventoryItem.GetItemName();
        _quantityText.Text = inventoryItem.GetItemQuantity().ToString();
    }
}
