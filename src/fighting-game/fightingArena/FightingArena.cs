using Godot;
using System;

public partial class FightingArena : Control
{
	[Export] PackedScene fightingActorScene;

	bool _isActive = false;

	AnimatedSprite2D _sprite;
	
	Vector2 _initialPosition;

	public enum States
	{
		Idle,
		
	}
	

    public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
	}

	public void Setup(
		int x,
		int y,
		SpriteFrames spriteFrames
	)
	{
		Vector2 newPosition = new Vector2(x, y);	
		Position = newPosition;
		_initialPosition = Position;

		_sprite.SpriteFrames = spriteFrames;
	}

	public void SetActive(bool setActive)
	{
		_isActive = setActive;
		SetProcessInput(_isActive);
	}
}
