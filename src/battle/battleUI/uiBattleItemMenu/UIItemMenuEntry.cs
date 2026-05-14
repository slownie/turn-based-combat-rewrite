using Godot;
using System;

public partial class UIItemMenuEntry : Control
{
    TextureRect _icon;
    Label _nameText;
    Label _quantityText;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("HBoxContainer/Icon");
        _nameText = GetNode<Label>("HBoxContainer/Name");
        _quantityText = GetNode<Label>("HBoxContainer/Quantity");
    }

    public void Setup(InventoryItem inventoryItem)
    {
        _icon.Texture = inventoryItem.GetIcon();
        _nameText.Text = inventoryItem.GetItemName();
        _quantityText.Text = inventoryItem.GetItemQuantity().ToString();
    }
}
