using Godot;
using System;

public partial class OverworldPlayer : CharacterBody2D
{
	[Signal] public delegate void OverworldMenuRequestedEventHandler();

	public const float Speed = 200.0f;

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("XButton"))
		{
			EmitSignal(SignalName.OverworldMenuRequested);
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			velocity = velocity.Normalized() * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
