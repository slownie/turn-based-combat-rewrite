using Godot;
using System;

public partial class HitEffect : AnimatedSprite2D
{
	[Signal] public delegate void HitEffectFinishedEventHandler();

	public void Setup(Vector2 position, SpriteFrames spriteFrames)
	{
		Position = position;
		SpriteFrames = spriteFrames;
		Play("default");

		AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished()
	{
		EmitSignal(SignalName.HitEffectFinished);
		QueueFree();
	}
}
