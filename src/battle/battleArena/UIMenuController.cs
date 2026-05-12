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

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.R)
			{
				GD.Print(_battleActors[2].GetCurHP());
			}
		}
	}


	public void BindServices(ActorController actorController, Godot.Collections.Array<BattleActor> battleActors)
	{
		_actorController = actorController;
		_battleActors = battleActors;
	}

	public void PartyTurnStart(BattleActor currentPartyActor)
	{
		_currentPartyActor = currentPartyActor;
		CreateMainMenu();
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

	private void CreateTargetCursor(UseableSkillResource selectedAction)
	{
		GD.Print("==Create Target Cursor==");
		Godot.Collections.Array<BattleActor> availableTargets = [];

		// Provide targeting parameters to TargetCursorController
		TargetingSettings targetingSettings = selectedAction.GetTargetingSettings();

		// 1. Are we targeting the party, enemies, both, or the self?
		if (targetingSettings.GetTargetOppositeSide())
		{
			availableTargets.AddRange(_actorController.GetEnemies(_battleActors));
		}

		if (targetingSettings.GetTargetSameSide())
		{
			availableTargets.AddRange(_actorController.GetPartyMembers(_battleActors));
		}

		if (targetingSettings.GetTargetSelfOnly())
		{
			availableTargets.Add(_currentPartyActor);
		}

		// 2. Are we targeting dead or alive actors?
		if (targetingSettings.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			availableTargets = _actorController.GetDeadActors(availableTargets);
		} else {
			availableTargets = _actorController.GetLiveActors(availableTargets);
		}

		// 3. Pass data to controller
		_uiTargetCursorController.Setup(availableTargets, targetingSettings.GetCursorMode());
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
