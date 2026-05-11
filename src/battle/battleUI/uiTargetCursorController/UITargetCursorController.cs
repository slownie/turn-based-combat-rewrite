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

	Godot.Collections.Array<BattleActor> _availableTargets = [];

	int _targetIndex = 0;

	bool _targetsAll = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
	{
		// Movement
        if (@event is InputEventKey && !_targetsAll)
        {
            // Dumb but 
            int desiredDown = @event.IsActionReleased("MoveDown") ? 1 : 0;
            int desiredUp = @event.IsActionReleased("MoveUp") ? 1 : 0;
            
            _targetIndex += desiredDown - desiredUp;
            if (_targetIndex > _availableTargets.Count - 1) _targetIndex = 0;
            if (_targetIndex < 0) _targetIndex = _availableTargets.Count - 1;
            
        }
	}


	public void Setup(Godot.Collections.Array<BattleActor> availableTargets)
	{
		_availableTargets = availableTargets;
	}

}
