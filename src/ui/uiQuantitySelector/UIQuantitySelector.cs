using Godot;
using System;

public partial class UIQuantitySelector : Control
{
	[Signal] public delegate void QuantitySelectedEventHandler(int selectedQuantity);
	[Signal] public delegate void CancelledEventHandler();

	int _upperLimit;
	int _lowerLimit;

	int _currentQuantity = 0;
	int currentQuantity
	{
		get { return _currentQuantity; }
		set
		{
			_currentQuantity = value;

			if (_upperLimit < _currentQuantity) _currentQuantity = _upperLimit;
			if (_currentQuantity < _lowerLimit) _currentQuantity = _lowerLimit;

			// Update UI
			_quantityLabel.Text = _currentQuantity.ToString();

			if (_currentQuantity == _upperLimit) {
				_rightArrow.Hide();
			} else if (_currentQuantity == _lowerLimit) {
				_leftArrow.Hide();
			} else {
				_rightArrow.Show();
				_leftArrow.Show();
			}
		}
	}

	TextureRect _leftArrow;
	TextureRect _rightArrow;
	Label _quantityLabel;
	
	public override void _Ready()
	{
		_leftArrow = GetNode<TextureRect>("HBoxContainer/LeftArrow");
		_rightArrow = GetNode<TextureRect>("HBoxContainer/RightArrow");
		_quantityLabel = GetNode<Label>("HBoxContainer/QuantityAmount");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("MoveRight")) { currentQuantity += 1; }
		if (@event.IsActionPressed("MoveLeft")) { currentQuantity -= 1; }

		if (@event.IsActionPressed("MoveUp")) { currentQuantity += 5; }
		if (@event.IsActionPressed("MoveDown")) { currentQuantity -= 5;}

		if (@event.IsActionPressed("AButton"))
		{
			EmitSignal(SignalName.QuantitySelected, currentQuantity);
		}

		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.Cancelled);
		}
	}

	public void Setup(int upperLimit, int lowerLimit, int defaultAmount=1)
	{
		_upperLimit = upperLimit;
		_lowerLimit = lowerLimit;

		currentQuantity = defaultAmount;
	}
}
