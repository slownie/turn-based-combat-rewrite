using Godot;
using System;

public partial class UIBattleText : Marker2D
{
	// How far the label will move upwards	
	[Export] double moveDistance = 48.0;

	// How long the label will be moving upwards
	[Export] double moveTime = 0.6;

	// How long it takes for the label to fade
	[Export] double fadeTime = 0.1;

	Label _label;

	Color transparentColor = new Color(1, 1, 1, 0);


	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
	}

	public void Setup(Vector2 origin, string text, Color textColor)
	{
		GlobalPosition = origin;
		_label.Text = text;
		_label.Modulate = textColor;

		// Vector2.Up * moveDistance
		Vector2 moveVector = new Vector2(0, (float)(-1 * moveDistance));

		// Move the label in an upwards direction
		Vector2 target = moveVector + _label.Position;
		Tween _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
		_tween.TweenProperty(_label, "position", target, moveTime);

		// Fade out at the end of its movement upwards
		_tween.Parallel().TweenProperty(
			this,
			"modulate",
			transparentColor, // Transparent
			fadeTime
		).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Linear).SetDelay(moveTime - fadeTime);

		// Free the label after it has faded out
		_tween.TweenCallback(Callable.From(QueueFree));

	}
}
