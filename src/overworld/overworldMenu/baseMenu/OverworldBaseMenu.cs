using Godot;
using System;

public partial class OverworldBaseMenu : UIOverworldMenuBase
{
	[Export] PackedScene menuEntryScene;
	[Export] PackedScene partyMenuEntryScene;
	
	
	Godot.Collections.Array<UIPartyMenuEntry> _partyEntries = [];

	VBoxContainer _menuContainer;
	VBoxContainer _partyContainer;
	UIGoldDisplay _goldDisplay;

	Label _currentOption;

	Godot.Collections.Array<UIBaseMenuEntry> _menuEntries = [];
	UIBaseMenuEntry _currentEntry;
	UIBaseMenuEntry currentEntry
	{
		get { return _currentEntry; }
		set
		{
			if (_currentEntry != null) _currentEntry.Highlighted = false;

			_currentEntry = value;
			_currentEntry.Highlighted = true;
		}
	}

	int _index;
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

    public override void _Ready()
	{
		_menuContainer = GetNode<VBoxContainer>("MenuEntries");
		_partyContainer = GetNode<VBoxContainer>("PartyEntries");

		_goldDisplay = GetNode<UIGoldDisplay>("UIGoldDisplay");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("MoveUp")) index -= 1;
		if (@event.IsActionPressed("MoveDown")) index += 1;
	}

	public void Setup(GameState gameState)
	{
		// Menu Setup
		UIBaseMenuEntry skillsEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(skillsEntry);
		skillsEntry.Setup(UIBaseMenuEntry.MainMenuType.Skills);
		_menuEntries.Add(skillsEntry);

		UIBaseMenuEntry itemsEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(itemsEntry);
		itemsEntry.Setup(UIBaseMenuEntry.MainMenuType.Items);
		_menuEntries.Add(itemsEntry);

		UIBaseMenuEntry equipmentEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(equipmentEntry);
		equipmentEntry.Setup(UIBaseMenuEntry.MainMenuType.Equipment);
		_menuEntries.Add(equipmentEntry);

		UIBaseMenuEntry partyEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(partyEntry);
		partyEntry.Setup(UIBaseMenuEntry.MainMenuType.Party);
		_menuEntries.Add(partyEntry);

		UIBaseMenuEntry settingsEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(settingsEntry);
		settingsEntry.Setup(UIBaseMenuEntry.MainMenuType.Settings);
		_menuEntries.Add(settingsEntry);

		UIBaseMenuEntry saveEntry = menuEntryScene.Instantiate() as UIBaseMenuEntry;
		_menuContainer.AddChild(saveEntry);
		saveEntry.Setup(UIBaseMenuEntry.MainMenuType.Save);
		_menuEntries.Add(saveEntry);

		// Party Setup
		foreach(ActivePartyMember partyMember in gameState.GetActivePartyMembers())
		{
			UIPartyMenuEntry partyMenuEntry = partyMenuEntryScene.Instantiate() as UIPartyMenuEntry;
			_partyContainer.AddChild(partyMenuEntry);

			partyMenuEntry.Setup(partyMember);
		}

		_goldDisplay.Setup(gameState.GetGoldAmount());

		index = 0;
	}
}
