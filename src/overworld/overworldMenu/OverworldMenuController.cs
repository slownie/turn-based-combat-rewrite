using Godot;
using System;

public partial class OverworldMenuController : Control
{
	// Maybe change these to GD.Loads?
	[Export] PackedScene _baseMenuScene;

	[Export] PackedScene _skillsMenuScene;
	[Export] PackedScene _itemsMenuScene;
	[Export] PackedScene _equipmentMenuScene;
	[Export] PackedScene _partyMenuScene;
	[Export] PackedScene _settingsMenuScene;
	[Export] PackedScene _saveMenuScene;

	[Signal] public delegate void CloseMenuRequestedEventHandler();

	Godot.Collections.Array<UIOverworldMenuBase> _menuStack = [];
	UIOverworldMenuBase _currentMenu;

	GameState _gameState;
    
	public override void _Ready()
	{
	}

	public void BindServices(GameState gameState)
	{
		_gameState = gameState;
	}

	public void OpenMenu()
	{
		CreateBaseMenu();
	}


	private void CreateBaseMenu()
	{
		OverworldBaseMenu overworldBaseMenu = _baseMenuScene.Instantiate() as OverworldBaseMenu;
		AddChild(overworldBaseMenu);
		overworldBaseMenu.Setup(_gameState);

		LoadMenu(overworldBaseMenu);

		overworldBaseMenu.EntrySelected += OnBaseMenuSelected;
		overworldBaseMenu.SelectionCancelled += OnBaseMenuCancelled;
	}

	private void CreateSkillsMenu()
	{
		OverworldSkillsMenu overworldSkillsMenu = _skillsMenuScene.Instantiate() as OverworldSkillsMenu;
		AddChild(overworldSkillsMenu);
		overworldSkillsMenu.Setup(_gameState.GetActivePartyMembers());

		LoadMenu(overworldSkillsMenu);

		overworldSkillsMenu.ExitMenu += UnloadMenu;
	}

	private void CreateItemsMenu()
	{
		OverworldItemsMenu overworldItemsMenu = _itemsMenuScene.Instantiate() as OverworldItemsMenu;
		AddChild(overworldItemsMenu);
		overworldItemsMenu.Setup(_gameState);

		LoadMenu(overworldItemsMenu);

		overworldItemsMenu.ExitMenu += UnloadMenu;
	}

	private void CreateEquipmentMenu()
	{
		OverworldEquipmentMenu overworldEquipmentMenu = _equipmentMenuScene.Instantiate() as OverworldEquipmentMenu;
		AddChild(overworldEquipmentMenu);
		overworldEquipmentMenu.Setup(_gameState);

		LoadMenu(overworldEquipmentMenu);

		overworldEquipmentMenu.CreateEquipSelectMenu += CreateEquipSelectMenu;
		overworldEquipmentMenu.ExitMenu += UnloadMenu;
	}

	private void CreateEquipSelectMenu(PackedScene equipMenuScene, int partyMemberIndex, EquipmentItemResource.EquipmentType equipmentType)
	{
		OverworldEquipSelectMenu overworldEquipSelectMenu = equipMenuScene.Instantiate() as OverworldEquipSelectMenu;
		AddChild(overworldEquipSelectMenu);
		overworldEquipSelectMenu.Setup(_gameState, partyMemberIndex, equipmentType);

		LoadMenu(overworldEquipSelectMenu);

		overworldEquipSelectMenu.ExitMenu += UnloadMenu;
	}

	private void OnBaseMenuSelected(UIBaseMenuEntry.MainMenuType mainMenuType)
	{
		switch(mainMenuType)
		{
			case UIBaseMenuEntry.MainMenuType.Skills:
			{
				CreateSkillsMenu();
				break;
			}

			case UIBaseMenuEntry.MainMenuType.Items:
			{
				CreateItemsMenu();
				break;
			}

			case UIBaseMenuEntry.MainMenuType.Equipment:
			{
				GD.Print("Equipment");
				CreateEquipmentMenu();
				break;
			}

			case UIBaseMenuEntry.MainMenuType.Party:
			{
				break;
			}

			case UIBaseMenuEntry.MainMenuType.Settings:
			{
				break;
			}

			case UIBaseMenuEntry.MainMenuType.Save:
			{
				break;
			}
		}
	}

	private void OnBaseMenuCancelled()
	{
		Cleanup();
		EmitSignal(SignalName.CloseMenuRequested);
	}

	private void LoadMenu(UIOverworldMenuBase overworldMenu)
	{
		if (_currentMenu != null) _currentMenu.Disable();
		_menuStack.Insert(0, overworldMenu);
		
		_currentMenu = _menuStack[0];
		_currentMenu.Enable();
	}

	private void UnloadMenu()
	{
		// Only unload the menu if we are not on the main menu
		if (1 < _menuStack.Count)
		{
			_currentMenu.QueueFree();
			_menuStack.RemoveAt(0);

			_currentMenu = _menuStack[0];
			_currentMenu.Enable();
		}
	}

	private void Cleanup()
	{
		// Destroy all menus/targetting
		foreach (Node node in GetChildren())
		{
			node.QueueFree();
		}

		_currentMenu = null;
		_menuStack = [];
	}
}
