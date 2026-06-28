using Godot;
using System;

public partial class UIBattleMenuEntryBase : Control
{
    [Export] protected Color disabledColor = new Color("#A9A9A9");

    protected bool _enabled = false;

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
