using Godot;
using System;

public partial class OverworldMenuController : Control
{
	[Export] PackedScene _baseMenuScene;

	Godot.Collections.Array<UIOverworldMenuBase> _menuStack = [];
	UIOverworldMenuBase _currentMenu;

	Vector2 _menuPosition = new Vector2(160, 16);

	GameState _gameState;
    
	public override void _Ready()
	{
	}

	public void BindServices(GameState gameState)
	{
		_gameState = gameState;
	}


	private void CreateBaseMenu()
	{
		OverworldBaseMenu overworldBaseMenu = _baseMenuScene.Instantiate() as OverworldBaseMenu;
		AddChild(overworldBaseMenu);


		LoadMenu(overworldBaseMenu);
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
