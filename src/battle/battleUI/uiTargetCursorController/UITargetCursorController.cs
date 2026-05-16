using Godot;
using System;

public partial class UITargetCursorController : UIBattleMenuBase
{
	[Export] PackedScene cursorScene;

	/*
        Emitted when the player has selected targets.
        If the player pressed 'B' instead, return to the previous menu.
        Cursor will destroy itself after this signal is emitted.
    */
	[Signal] public delegate void TargetsSelectedEventHandler(Godot.Collections.Array<BattleActor> selectedTargets);
    [Signal] public delegate void TargetsCancelledEventHandler();

	// Targeting Parameters
	Godot.Collections.Array<BattleActor> _partyTargets = [];
	Godot.Collections.Array<BattleActor> _enemyTargets = [];

	// Targeting parameters
	BattleConsts.CursorMode _cursorMode = BattleConsts.CursorMode.Single;
	int _targetSide = 0; // 0 -> Enemy, 1 -> Party

	bool _canMoveTarget = true;
	bool _canMoveSide = false;

	Godot.Collections.Array<UITargetCursor> _partyCursors = [];
	Godot.Collections.Array<UITargetCursor> _enemyCursors = [];

	// Easier reading/composition
	Godot.Collections.Array<BattleActor> _currentTargets = []; 
	Godot.Collections.Array<UITargetCursor> _currentCursors = [];
	
	// Used only for single target
	int _currentIndex = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessInput(false);
	}

    public override void _Input(InputEvent @event)
	{
		// Single target only code
		if (_canMoveTarget)
		{
			float desiredVertical = Input.GetAxis("MoveUp", "MoveDown");
			if (desiredVertical != 0)
			{
				_currentCursors[_currentIndex].SetIsVisible(false);

				_currentIndex += (int)desiredVertical;
				if (_currentIndex < 0) _currentIndex = _currentCursors.Count - 1;
				if (_currentCursors.Count - 1 < _currentIndex) _currentIndex = 0;
				_currentCursors[_currentIndex].SetIsVisible(true);
			}
			
		}

		if (_canMoveSide)
		{
			bool moveRequested = @event.IsActionReleased("MoveLeft") || @event.IsActionReleased("MoveRight");
			if (moveRequested)
			{
				// Swap to player side
				if (_targetSide == 0)
				{
					_currentTargets = _partyTargets;
					_currentCursors[_currentIndex].SetIsVisible(false);

					_currentCursors = _partyCursors;
					if (_cursorMode == BattleConsts.CursorMode.Single)
					{

						if (_currentTargets.Count - 1 < _currentIndex) _currentIndex = _currentTargets.Count - 1;
						_currentCursors[_currentIndex].SetIsVisible(true);
					} else {
						foreach(UITargetCursor cursor in _currentCursors)
						{
							cursor.SetIsVisible(false);
						}
						_currentCursors = _partyCursors;
						foreach(UITargetCursor cursor in _currentCursors)
						{
							cursor.SetIsVisible(true);
						}
					}

					// Switch to player side
					_targetSide = 1;	
				} else {
					// Switch to enemy side
					_currentTargets = _enemyTargets;
					_currentCursors[_currentIndex].SetIsVisible(false);

					_currentCursors = _enemyCursors;
					if (_cursorMode == BattleConsts.CursorMode.Single)
					{
						if (_currentTargets.Count - 1 < _currentIndex) _currentIndex = _currentTargets.Count - 1;
						_currentCursors[_currentIndex].SetIsVisible(true);
					} else {
						foreach(UITargetCursor cursor in _currentCursors)
						{
							cursor.SetIsVisible(false);
						}
						_currentCursors = _enemyCursors;
						foreach(UITargetCursor cursor in _currentCursors)
						{
							cursor.SetIsVisible(true);
						}
					}

					_targetSide = 0;
				}
			}
		}
	
		// Select Targets
		if (@event.IsActionPressed("AButton"))
		{
			switch (_cursorMode)
			{
				case BattleConsts.CursorMode.Single:
				{
					// Stupid but this is how it works
					Godot.Collections.Array<BattleActor> tempArray = [_currentTargets[_currentIndex]];
					
					EmitSignal(SignalName.TargetsSelected, tempArray);
					break;
				}

				case BattleConsts.CursorMode.Side:
				{
					GD.Print("side");
					if (_targetSide == 0)
					{
						EmitSignal(SignalName.TargetsSelected, _enemyTargets);
					} else {
						EmitSignal(SignalName.TargetsSelected, _partyTargets);
					}
					break;
				}

				case BattleConsts.CursorMode.All:
				{
					GD.Print("all");
					EmitSignal(SignalName.TargetsSelected, _currentTargets);
					break;
				}
			}
			
		}

		// Quit Selection
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.TargetsCancelled);
		}
	}

	public void Setup(
		Godot.Collections.Array<BattleActor> partyTargets, 
		Godot.Collections.Array<BattleActor> enemyTargets, 
		BattleConsts.CursorMode cursorMode
	)
	{
		_partyTargets = partyTargets;
		_enemyTargets = enemyTargets;
		_cursorMode = cursorMode;

		// Setup & create based on cursor mode & targets
		switch (_cursorMode)
		{
			case BattleConsts.CursorMode.Single:
			{
				// Move between targets, only allow side movement if targets exist in both
				_canMoveTarget = true;
				_canMoveSide = _partyTargets.Count != 0 && _enemyTargets.Count != 0 ? true : false;
				_currentIndex = 0;

				// Default side
				if (_partyTargets.Count != 0 && _enemyTargets.Count == 0)
				{
					// Go to party
					_currentTargets = _partyTargets;
				} else {
					// Default to enemy
					_currentTargets = _enemyTargets;
				}

				// Create Cursors
				foreach(BattleActor enemyActor in enemyTargets)
				{
					UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
					AddChild(targetCursor);

					_enemyCursors.Add(targetCursor);

					targetCursor.SetIsVisible(enemyActor == _currentTargets[_currentIndex]);
					targetCursor.Position = enemyActor.Position;
				}

				foreach(BattleActor partyActor in partyTargets)
				{
					UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
					AddChild(targetCursor);

					_partyCursors.Add(targetCursor);

					targetCursor.SetIsVisible(partyActor == _currentTargets[_currentIndex]);
					targetCursor.Position = partyActor.Position;
				}

				// Targeting party only
				if (_partyCursors.Count != 0 && _enemyCursors.Count == 0)
				{
					_targetSide = 1;
					_currentCursors = _partyCursors;
				} else {
					_currentCursors = _enemyCursors;
				}

				break;
			}

			case BattleConsts.CursorMode.Side:
			{
				// No target movement, only allow side movement if targets exist in both
				_canMoveTarget = false;
				_canMoveSide = _partyTargets.Count != 0 && _enemyTargets.Count != 0 ? true : false;

				// Default side
				if (_partyTargets.Count != 0 && _enemyTargets.Count == 0)
				{
					// Go to party
					_currentTargets = _partyTargets;
				} else {
					// Default to enemy
					_currentTargets = _enemyTargets;
				}

				// Create Cursors
				foreach(BattleActor enemyActor in enemyTargets)
				{
					UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
					AddChild(targetCursor);

					_enemyCursors.Add(targetCursor);

					targetCursor.SetIsVisible(_currentTargets.Contains(enemyActor));
					targetCursor.Position = enemyActor.Position;
				}

				foreach(BattleActor partyActor in partyTargets)
				{
					UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
					AddChild(targetCursor);

					_partyCursors.Add(targetCursor);

					targetCursor.SetIsVisible(_currentTargets.Contains(partyActor));
					targetCursor.Position = partyActor.Position;
				}

				// Targeting party only
				if (_partyCursors.Count != 0 && _enemyCursors.Count == 0)
				{
					_targetSide = 1;
					_currentCursors = _partyCursors;
				} else {
					_currentCursors = _enemyCursors;
				}

				break;
			}

			case BattleConsts.CursorMode.All:
			{
				// No movement
				_canMoveTarget = false;
				_canMoveSide = false;

				// Target all actors
				_currentTargets.AddRange(_partyTargets);
				_currentTargets.AddRange(_enemyTargets);

				// Create Cursors
				foreach (BattleActor actor in _currentTargets)
				{
					UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
					AddChild(targetCursor);

					_currentCursors.Add(targetCursor);

					targetCursor.Position = actor.Position;
				}
				break;
			}
		}

		// _canMoveSide = _partyTargets.Count !=0 && _enemyTargets.Count !=0 ? true : false;

		// if (_cursorMode == BattleConsts.CursorMode.Single)
		// {
		// 	if (partyTargets.Count != 0 && enemyTargets.Count == 0)
		// 	{
		// 		_currentTargets = partyTargets;
		// 		_currentTarget = _currentTargets[0];
		// 	} else {
		// 		// Default to enemy
		// 		_currentTargets = enemyTargets;
		// 		_currentTarget = _currentTargets[0];
		// 	}
		// }

		// // Create Cursors
		// foreach(BattleActor enemyActor in enemyTargets)
		// {
		// 	UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
		// 	AddChild(targetCursor);

		// 	_enemyCursors.Add(targetCursor);

		// 	targetCursor.SetIsVisible(_currentTarget == enemyActor);
		// 	targetCursor.Position = enemyActor.Position;
		// }


		// foreach(BattleActor partyActor in partyTargets)
		// {
		// 	UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
		// 	AddChild(targetCursor);

		// 	_partyCursors.Add(targetCursor);

		// 	targetCursor.SetIsVisible(_currentTarget == partyActor);
		// 	targetCursor.Position = partyActor.Position;
		// }

		// _currentCursors = _currentTargets == _partyTargets ? _partyCursors : _enemyCursors;

		SetProcessInput(true);
		

	}

	public void Cleanup()
	{
		// Reset variables and destroy all cursors

		foreach (UITargetCursor cursor in GetChildren())
		{
			cursor.QueueFree();
		}

		SetProcessInput(false);
	}
}
