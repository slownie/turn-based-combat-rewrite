using Godot;
using System;

public partial class UIMenuController : Control
{
	[Export] PackedScene mainMenuScene;
	[Export] PackedScene actionSelectionMenuScene;
	[Export] PackedScene targetingCursorScene;

	[Signal] public delegate void TargetSelectedEventHandler();

	BattleActor _currentPartyActor;
	Control _loadedMenu;

    public override void _Ready()
    {
    }

	public void PartyTurnStart(BattleActor currentPartyActor)
	{
		_currentPartyActor = currentPartyActor;
		CreateMainMenu();
	}

	private void CreateTargetCursor(UseableSkillResource selectedAction)
	{
		GD.Print("Target Cursor Created");
	}

	private void CreateMainMenu()
	{
		UIBattleMainMenu mainMenu = mainMenuScene.Instantiate() as UIBattleMainMenu;
		AddChild(mainMenu);

		mainMenu.ActionSelected += CreateTargetCursor;
		mainMenu.SkillMenuRequested += CreateSkillMenu;
		mainMenu.ItemMenuRequested += CreateItemMenu;

		mainMenu.Setup(true, true);
	}

	private void CreateSkillMenu()
	{
		GD.Print("Skill Menu Created");
	}

	private void CreateItemMenu()
	{
		GD.Print("Item Menu Created");		
	}

	private void Cleanup()
	{
		// Destroy all menus/targetting
		foreach (Node node in GetChildren())
		{
			node.QueueFree();
		}

		_currentPartyActor = null; 
	}
}
