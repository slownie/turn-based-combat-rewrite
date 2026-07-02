using Godot;
using System;

public partial class BattleActorAnim : Marker2D
{
	Sprite2D _sprite;
	AnimationPlayer _animationplayer;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite");
		_animationplayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationplayer.AnimationFinished += OnAnimationFinished;
	}

	public void Setup(bool isPlayer)
	{
		if (isPlayer)
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

}
