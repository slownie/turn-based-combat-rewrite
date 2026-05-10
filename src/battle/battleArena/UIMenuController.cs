using Godot;
using System;

public partial class UIMenuController : Control
{
	[Export] PackedScene mainMenuScene;
	[Export] PackedScene actionSelectionMenuScene;

	[Signal] public delegate void TargetSelectedEventHandler();

	UITargetCursorController _uiTargetCursorController;

	ActorController _actorController;
	Godot.Collections.Array<BattleActor> _battleActors = [];

	BattleActor _currentPartyActor;

    public override void _Ready()
	{
		_uiTargetCursorController = GetNode<UITargetCursorController>("UITargetCursorController");
	}

	public void BindServices(ActorController actorController)
	{
		_actorController = actorController;
	}

	public void PartyTurnStart(BattleActor currentPartyActor)
	{
		_currentPartyActor = currentPartyActor;
		CreateMainMenu();
	}

	private void CreateTargetCursor(UseableSkillResource selectedAction)
	{
		Godot.Collections.Array<BattleActor> availableTargets = [];
		//availableTargets = _actorController.GetLiveActors();
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
