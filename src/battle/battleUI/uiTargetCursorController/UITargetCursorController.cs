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

	bool _targetsAll = false;

	int _targetIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Input(InputEvent @event)
    {
    }


	public void Setup(bool targetsAll)
	{
		_targetsAll = targetsAll;
	}

}
