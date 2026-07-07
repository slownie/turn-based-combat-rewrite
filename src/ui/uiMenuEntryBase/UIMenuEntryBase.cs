using Godot;
using System;

public partial class UIMenuEntryBase : Control
{
	[Export] protected Color defaultColor = new Color("#ffffff");
    [Export] protected Color choiceColor = new Color("#fbff00");
    [Export] protected Color disabledColor = new Color("#A9A9A9");

	protected TextureRect _icon;
	protected Label _text;

	protected bool _enabled = false;

    bool highlighted = false;
	public bool Highlighted
	{
		get { return highlighted; }
		set
		{
			highlighted = value;
			if (highlighted)
			{
				_text.Modulate = choiceColor;
			} else {
                if (_enabled)
                {
                    _text.Modulate = defaultColor;
                } else {
                    _text.Modulate = disabledColor;
                }
			}
		}
	}

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("HBoxContainer/Icon");
		_text = GetNode<Label>("HBoxContainer/Text");
	}
}
