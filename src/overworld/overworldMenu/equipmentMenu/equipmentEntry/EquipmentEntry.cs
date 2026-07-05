using Godot;
using System;

public partial class EquipmentEntry : Control
{
    Label _textLabel;

    [Export] protected Color defaultColor = new Color("#ffffff");
    [Export] protected Color choiceColor = new Color("#fbff00");
    [Export] protected Color disabledColor = new Color("#A9A9A9");
    
    [Export] EquipmentItemResource.EquipmentType equipmentType;

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
				_textLabel.Modulate = choiceColor;
			} else {
                if (_enabled)
                {
                    _textLabel.Modulate = defaultColor;
                } else {
                    _textLabel.Modulate = disabledColor;
                }
			}
		}
	}



    public override void _Ready()
    {
        _textLabel = GetNode<Label>("HBoxContainer/Text");
    }

    public void SetText(string text)
    {
        _textLabel.Text = text;
    }

    public EquipmentItemResource.EquipmentType GetEquipmentType() { return equipmentType; }
}
