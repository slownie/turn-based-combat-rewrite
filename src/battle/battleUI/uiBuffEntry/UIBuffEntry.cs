using Godot;
using System;

public partial class UIBuffEntry : HBoxContainer
{
    TextureRect _icon;
    Label _turnCount;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("Icon");
        _turnCount = GetNode<Label>("TurnCount");
    }

    public void Setup(BattleActor battleActor)
    {
        
    }
}
