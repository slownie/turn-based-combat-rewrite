using Godot;
using System;

public partial class ScreenTransition : CanvasLayer
{
	Tween _tween = null;
	ColorRect _colorRect;

    public override void _Ready()
	{
		_colorRect = GetNode<ColorRect>("ColorRect");
		Hide();
	}

	public void StartTransition()
	{
		if (_tween != null) _tween.Kill();
		_tween = CreateTween().SetTrans(Tween.TransitionType.Sine);
		_tween.TweenProperty(_colorRect, "modulate:a", 1.0, 0.5);
		_tween.TweenInterval(0.5f);
		_tween.TweenProperty(_colorRect, "modulate:a", 0.0, 0.5);
	}
}
