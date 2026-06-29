using Godot;
using System;

public partial class UIGoldDisplay : Control
{
	Label _goldAmount;

	public override void _Ready()
	{
		_goldAmount = GetNode<Label>("HBoxContainer/GoldAmount");
	}

	public void Setup(int goldAmount)
	{
		_goldAmount.Text = goldAmount.ToString();
	}
}
