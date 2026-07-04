using Godot;
using System;

public partial class ActionBox : HBoxContainer
{
	TextureRect _actionIcon;
	Label _actionName;

	public override void _Ready()
	{
		_actionIcon = GetNode<TextureRect>("ActionIcon");
		_actionName = GetNode<Label>("ActionName");
	}

	public void Setup(string actionName, Texture2D actionIcon)
	{
		_actionName.Text = actionName;
		_actionIcon.Texture = actionIcon;
		Show();
	}

	public void Reset()
	{
		_actionName.Text = "";
		_actionIcon.Texture = null;
		Hide();
	}
}
