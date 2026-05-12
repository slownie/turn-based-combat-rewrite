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
	Godot.Collections.Array<BattleActor> _availableTargets = [];
	BattleActor _currentTarget;

	BattleConsts.CursorMode _cursorMode = BattleConsts.CursorMode.Single;
	int _targetIndex = 0;

	Godot.Collections.Array<UITargetCursor> _cursors = [];


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
	{
		// Movement
        if (@event is InputEventKey)
        {
            // Dumb but 
            int desiredDown = @event.IsActionReleased("MoveDown") ? 1 : 0;
            int desiredUp = @event.IsActionReleased("MoveUp") ? 1 : 0;
            
            _targetIndex += desiredDown - desiredUp;
            if (_targetIndex > _availableTargets.Count - 1) _targetIndex = 0;
            if (_targetIndex < 0) _targetIndex = _availableTargets.Count - 1;
			_currentTarget = _availableTargets[_targetIndex];
        }
	}


	public void Setup(
		Godot.Collections.Array<BattleActor> availableTargets,
		BattleConsts.CursorMode cursorMode,
		bool enableMovement=false
	)
	{
		GD.Print("==Cursor Controller Setup==");
		_cursorMode = cursorMode;
		_currentTarget = availableTargets[0];

		/*
		if (partyTargets.Count != 0 && enemyTargets.Count == 0)
		{
			_currentTarget = partyTargets[0];
		} else {
			// Default to enemy
			_currentTarget = enemyTargets[0];
		}

		// Create Cursors
		foreach(BattleActor partyActor in partyTargets)
		{
			UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
			AddChild(targetCursor);

			targetCursor.SetIsVisible(_currentTarget == partyActor);
			targetCursor.Position = partyActor.Position;
		}

		foreach(BattleActor enemyActor in enemyTargets)
		{
			UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
			AddChild(targetCursor);

			targetCursor.SetIsVisible(_currentTarget == enemyActor);
			targetCursor.Position = enemyActor.Position;
		}
		*/

		_availableTargets = availableTargets;

		// Specify number of cursors to create
		switch(_cursorMode)
		{
			// Only create the one cursor, default to the first actor in the array
			case BattleConsts.CursorMode.Single:
			{
				GD.Print("Cursor Mode - Single");
				UITargetCursor targetCursor = cursorScene.Instantiate() as UITargetCursor;
				AddChild(targetCursor);

				_cursors.Add(targetCursor);

				
				targetCursor.CurrentTarget = _currentTarget;

				break;
			}
		}

		

	}

	public void Cleanup()
	{
		// Reset variables and destroy all cursors

		foreach (UITargetCursor cursor in GetChildren())
		{
			cursor.QueueFree();
		}
	}

}
