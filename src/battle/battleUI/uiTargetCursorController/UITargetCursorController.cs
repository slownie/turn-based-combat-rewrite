using Godot;
using System;

public partial class UITargetCursorController : Node2D
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
	bool _canMoveSide = false;
	Godot.Collections.Array<UITargetCursor> _partyCursors = [];
	Godot.Collections.Array<UITargetCursor> _enemyCursors = [];

	// Easier reading/composition
	Godot.Collections.Array<BattleActor> _currentTargets = []; 
	Godot.Collections.Array<UITargetCursor> _currentCursors = [];
	
	// Used only for single target
	BattleActor _currentTarget;
	int _currentIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessInput(false);
	}

    public override void _Input(InputEvent @event)
	{
		// Movement
        if (@event is InputEventKey)
        {
			if (_cursorMode == BattleConsts.CursorMode.Single)
			{
				// Dumb but 
				int desiredDown = @event.IsActionReleased("MoveDown") ? 1 : 0;
				int desiredUp = @event.IsActionReleased("MoveUp") ? 1 : 0;
				
				// Hide the current cursor
				_currentCursors[_currentIndex].SetIsVisible(false);

				_currentIndex += desiredDown - desiredUp;
				if (_currentIndex > _currentTargets.Count - 1) _currentIndex = 0;
				if (_currentIndex < 0) _currentIndex = _currentTargets.Count - 1;

				_currentTarget = _currentTargets[_currentIndex];
				_currentCursors[_currentIndex].SetIsVisible(true);
			}
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

		if (_cursorMode == BattleConsts.CursorMode.Single)
		{
			if (partyTargets.Count != 0 && enemyTargets.Count == 0)
			{
				_currentTargets = partyTargets;
				_currentTarget = _currentTargets[0];
			} else {
				// Default to enemy
				_currentTargets = enemyTargets;
				_currentTarget = _currentTargets[0];
			}
		}

		// Create Cursors
		foreach(BattleActor enemyActor in enemyTargets)
		{
			UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
			AddChild(targetCursor);

			_enemyCursors.Add(targetCursor);

			targetCursor.SetIsVisible(_currentTarget == enemyActor);
			targetCursor.Position = enemyActor.Position;
		}


		foreach(BattleActor partyActor in partyTargets)
		{
			UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
			AddChild(targetCursor);

			_partyCursors.Add(targetCursor);

			targetCursor.SetIsVisible(_currentTarget == partyActor);
			targetCursor.Position = partyActor.Position;
		}

		_currentCursors = _currentTargets == _partyTargets ? _partyCursors : _enemyCursors;

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
