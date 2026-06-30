using Godot;
using System;

public partial class UIBattleMenuEntryBase : Control
{
    [Export] protected Color defaultColor = new Color("#ffffff");
    [Export] protected Color choiceColor = new Color("#fbff00");
    [Export] protected Color disabledColor = new Color("#A9A9A9");
    
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
				_nameText.Modulate = choiceColor;
			} else {
                if (_enabled)
                {
                    _nameText.Modulate = defaultColor;
                } else {
                    _nameText.Modulate = disabledColor;
                }
			}
		}
	}

    protected TextureRect _icon;
    protected Label _nameText;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("HBoxContainer/Icon");
        _nameText = GetNode<Label>("HBoxContainer/Name");
    }

    public bool IsEnabled() { return _enabled; }

    public virtual string GetMenuDesc() { return ""; }
}
