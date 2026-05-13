using Godot;
using System;

public partial class UIMenuController : Control
{
	[Export] PackedScene mainMenuScene;
	[Export] PackedScene actionSelectionMenuScene;
	[Export] PackedScene targetMenuScene;

	[Signal] public delegate void TargetSelectedEventHandler();

	ActorController _actorController;
	Godot.Collections.Array<BattleActor> _battleActors = [];

	BattleActor _currentPartyActor;

	Godot.Collections.Array<UIBattleMenuBase> _menuStack = [];
	UIBattleMenuBase _currentMenu;

    public override void _Ready()
	{
		
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
		LoadMenu(mainMenu);
	}

	private void CreateTargetCursor(UseableSkillResource selectedAction)
	{
		UITargetCursorController targetMenu = targetMenuScene.Instantiate() as UITargetCursorController;
		AddChild(targetMenu);

		targetMenu.TargetsSelected += OnTargetsSelected;

		Godot.Collections.Array<BattleActor> _partyTargets = [];
		Godot.Collections.Array<BattleActor> _enemyTargets = [];

		// Provide targeting parameters to TargetCursorController
		TargetingSettings targetingSettings = selectedAction.GetTargetingSettings();

		// 1. Are we targeting the party, enemies, both, or the self?
		if (targetingSettings.GetTargetOppositeSide())
		{
			_enemyTargets = _actorController.GetEnemies(_battleActors);
		}

		if (targetingSettings.GetTargetSameSide())
		{
			_partyTargets = _actorController.GetPartyMembers(_battleActors);
		}

		if (targetingSettings.GetTargetSelfOnly())
		{
			_partyTargets.Add(_currentPartyActor);
		}

		// 2. Are we targeting dead or alive actors?
		if (targetingSettings.GetTargetDeadOnly())
		{
			// We are only targeting dead party members
			_partyTargets = _actorController.GetDeadActors(_partyTargets);
		} else {
			if (_enemyTargets.Count != 0) _enemyTargets = _actorController.GetLiveActors(_enemyTargets);
			if (_partyTargets.Count != 0) _partyTargets = _actorController.GetLiveActors(_partyTargets);
		}

		// 3. Pass data to controller
		targetMenu.Setup(_partyTargets, _enemyTargets, targetingSettings.GetCursorMode());
		LoadMenu(targetMenu);
	}

	private void CreateSkillMenu()
	{
		GD.Print("Skill Menu Created");
	}

	private void CreateItemMenu()
	{
		GD.Print("Item Menu Created");		
	}

	private void OnTargetsSelected(Godot.Collections.Array<BattleActor> selectedActors)
	{
		Cleanup();
	}

	private void LoadMenu(UIBattleMenuBase battleMenu)
	{
		if (_currentMenu != null) _currentMenu.Disable();
		_menuStack.Insert(0, battleMenu);
		
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

		_currentPartyActor = null; 
	}
}
