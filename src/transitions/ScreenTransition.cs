using Godot;
using System;

public partial class ScreenTransition : CanvasLayer
{
	[Signal] public delegate void FadeOutFinishedEventHandler();
	[Signal] public delegate void FadeInFinishedEventHandler();

	Tween _tween = null;
	ColorRect _colorRect;

    public override void _Ready()
	{
		_colorRect = GetNode<ColorRect>("ColorRect");
	}

	public void FadeOutTransition()
	{
		if (_tween != null) _tween.Kill();
		_tween = CreateTween().SetTrans(Tween.TransitionType.Sine);
		_tween.TweenProperty(_colorRect, "modulate:a", 1.0, 0.5);
		_tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.FadeOutFinished)));
	}

	public void FadeInTransition()
	{
		if (_tween != null) _tween.Kill();
		_tween = CreateTween().SetTrans(Tween.TransitionType.Sine);
		_tween.TweenProperty(_colorRect, "modulate:a", 0.0, 0.5);
		_tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.FadeInFinished)));
	}
}
