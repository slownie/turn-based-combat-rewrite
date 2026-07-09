using Godot;
using System;

public partial class EquipItemEntry : Control
{
	[Export] protected Color defaultColor = new Color("#ffffff");
    [Export] protected Color choiceColor = new Color("#fbff00");
    [Export] protected Color disabledColor = new Color("#A9A9A9");

	TextureRect _itemIcon;
	Label _itemName;
	TextureRect _equipperIcon;

	EquipmentItem _storedItem;

	protected bool _enabled = true;

    bool highlighted = false;
	public bool Highlighted
	{
		get { return highlighted; }
		set
		{
			highlighted = value;
			if (highlighted)
			{
				_itemName.Modulate = choiceColor;
			} else {
                if (_enabled)
                {
                    _itemName.Modulate = defaultColor;
                } else {
                    _itemName.Modulate = disabledColor;
                }
			}
		}
	}

	public override void _Ready()
	{
		_itemIcon = GetNode<TextureRect>("HBoxContainer/ItemIcon");
		_itemName = GetNode<Label>("HBoxContainer/ItemName");
		_equipperIcon = GetNode<TextureRect>("HBoxContainer/EquipperIcon");
	}

	public void Setup(EquipmentItem storedItem)
	{
		_storedItem = storedItem;
		GD.Print(_storedItem);
		if (_storedItem != null)
		{
			_itemName.Text = _storedItem.GetItemName();

			if (_storedItem.GetUser() != null)
			{
				_equipperIcon.Texture = _storedItem.GetUser().GetBattleIcon();
			}
		} else {
			// 'Empty' item entry
			_itemName.Text = "Empty";
			_enabled = false;
		}
	}

	public EquipmentItem GetEquipmentItem()
	{
		return _storedItem;
	}

	public bool GetEnabled() { return _enabled; }
}
