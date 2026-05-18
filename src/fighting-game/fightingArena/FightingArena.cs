using Godot;
using System;

public partial class FightingArena : Control
{
	[Export] PackedScene fightingActorScene;

	bool _isActive = false;

	AnimatedSprite2D _sprite;
	
	public void SetActive(bool setActive)
	{
		_isActive = setActive;
		SetProcessInput(_isActive);
	}

    public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
	}
}
