using Godot;
using System;

public partial class OverworldItemsMenu : UIOverworldMenuBase
{
	[Export] PackedScene itemMenuEntryScene;
	[Export] PackedScene partyMenuEntryScene;

	Godot.Collections.Array<UIPartyMenuEntry> _partyEntries = [];

	VBoxContainer _menuContainer;
	VBoxContainer _partyContainer;

	Godot.Collections.Array<UIItemMenuEntry> _menuEntries = [];
	UIItemMenuEntry _currentEntry;
	UIItemMenuEntry currentEntry
	{
		get { return _currentEntry; }
		set
		{
			if (_currentEntry != null) _currentEntry.Highlighted = false;

			_currentEntry = value;
			_currentEntry.Highlighted = true;
		}
	}

	int _index = 0;
	int index
	{
		get { return _index; }
		set
		{
			_index = value;
			if (_menuEntries.Count <= _index) _index = 0;
			if (_index < 0) _index = _menuEntries.Count - 1;
			currentEntry = _menuEntries[_index];
		}
	}

	int _tabIndex = 0;
	int tabIndex
	{
		get { return _tabIndex; }
		set
		{
			_tabIndex = value;
		}
	}


    public override void _Ready()
	{
		_menuContainer = GetNode<VBoxContainer>("MenuEntries");
		_partyContainer = GetNode<VBoxContainer>("PartyEntries");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("MoveUp")) index -= 1;
		if (@event.IsActionPressed("MoveDown")) index += 1;

		// Use
		if (@event.IsActionPressed("AButton"))
		{
			// Are we allowed to select this option?
			if (currentEntry.IsEnabled())
			{
				
			}
		}

		if (@event.IsActionPressed("BButton")) EmitSignal(SignalName.ExitMenu);
	
		// Discard
		if (@event.IsActionPressed("YButton"))
		{
			if (currentEntry.IsEnabled())
			{
				
			}
		}
	}

	public void Setup(GameState gameState, InventoryController inventoryController)
	{
		foreach (InventoryItem inventoryItem in gameState.GetInventoryItems())
		{
			UIItemMenuEntry itemEntry = itemMenuEntryScene.Instantiate() as UIItemMenuEntry;
			_menuContainer.AddChild(itemEntry);

			itemEntry.Setup(inventoryItem);
			_menuEntries.Add(itemEntry);
		}

		// Party Setup
		foreach(ActivePartyMember partyMember in gameState.GetActivePartyMembers())
		{
			UIPartyMenuEntry partyMenuEntry = partyMenuEntryScene.Instantiate() as UIPartyMenuEntry;
			_partyContainer.AddChild(partyMenuEntry);

			partyMenuEntry.Setup(partyMember);
		}

		index = 0;
	}
}
