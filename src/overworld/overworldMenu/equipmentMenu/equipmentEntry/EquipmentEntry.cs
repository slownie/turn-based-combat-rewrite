using Godot;
using System;

public partial class EquipmentEntry : Control
{
    Label _textLabel;

    public override void _Ready()
    {
        _textLabel = GetNode<Label>("HBoxContainer/Text");
    }

    public void SetText(string text)
    {
        _textLabel.Text = text;
    }
}
