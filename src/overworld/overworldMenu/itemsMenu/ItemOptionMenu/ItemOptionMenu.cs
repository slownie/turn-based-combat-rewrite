using Godot;
using System;

public partial class ItemOptionMenu : Control
{
	[Export] PackedScene menuEntryScene;

	[Signal] public delegate void UseSelectedEventHandler();
	[Signal] public delegate void DiscardSelectedEventHandler();
	[Signal] public delegate void CancelledEventHandler();

	VBoxContainer _choiceContainer;

	UIMenuEntryBase _currentEntry;

    public override void _Ready()
	{
		_choiceContainer = GetNode<VBoxContainer>("VBoxContainer");
	}

    public override void _Input(InputEvent @event)
	{
		
	}

	public void Setup()
	{
		UIMenuEntryBase useEntry = menuEntryScene.Instantiate() as UIMenuEntryBase;
		_choiceContainer.AddChild(useEntry);

		
	}
}
