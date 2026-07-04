using Godot;
using System;

public partial class BattleActorAnim : Marker2D
{
	Sprite2D _sprite;
	AnimationPlayer _animationplayer;

	bool _isPlayer;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite");
		_animationplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationplayer.AnimationFinished += OnAnimationFinished;
	}

	public void Setup(bool isPlayer)
	{
		_isPlayer = isPlayer;
		if (_isPlayer)
		{
			_sprite.FlipH = true;
		}
	}

	public void PlayAnimation(string animationName)
	{
		_animationplayer.Play(animationName);
	}

	private void OnAnimationFinished(StringName animationName)
	{
		_animationplayer.Play("idle");
	}

	public void SetFlipX(bool value)
	{
		if (_isPlayer) _sprite.FlipH = !value;
		else _sprite.FlipH = value;
	}

}
