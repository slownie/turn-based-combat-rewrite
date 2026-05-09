using Godot;
using System;

public partial class UIBar : TextureProgressBar
{
	// Rate of the animation relative to the maxValue
	// 1.0 means the animation fills the entire bar in one second
	[Export] double fillRate = 0.5;
	[Export] double dangerCutoff = 0.2;

	Tween tween = null;

	// Animate towards this value using a tween
	double _targetValue = 0.0;
	public double targetValue
	{
		get {return _targetValue;}
		set
		{
			_targetValue = value;
			if (tween != null) tween.Kill();

			float duration = (float)(Math.Abs(_targetValue - value) / MaxValue * fillRate);
			tween = CreateTween().SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(this, "value", _targetValue, duration);
		}
	}

	Label valueLabel;
	Label maxLabel;

    public override void _Ready()
	{
		valueLabel = GetNode<Label>("MarginContainer/HBoxContainer/Value");
		maxLabel = GetNode<Label>("MarginContainer/HBoxContainer/Max");
		ValueChanged += OnValueChanged;
	}

	public void Setup(int startingValue, int barMax)
	{
		Value = startingValue;
		MaxValue = barMax;
		maxLabel.Text = barMax.ToString();
	}

	private void OnValueChanged(double newValue)
	{
		valueLabel.Text = newValue.ToString();
	}
}
