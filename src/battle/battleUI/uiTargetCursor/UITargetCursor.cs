using Godot;
using System;

public partial class UITargetCursor : Marker2D
{
	const double SlideTime = 0.1;

	Tween slideTween = null;

	/*
		Smootly move the cursor to the specified position.
		Called by the menu to move the cursor from entry to entry.
	*/
	public void MoveTo(Vector2 target)
	{
		if (slideTween != null) slideTween.Kill();
		slideTween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		slideTween.TweenProperty(this, "position", target, SlideTime);
	}

	public void SetIsVisible(bool isVisible)
	{
		if (isVisible)
		{
			Show();
		} else {
			Hide();
		}
	}
}
