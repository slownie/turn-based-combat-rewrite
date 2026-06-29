using Godot;
using System;

public partial class OverworldController : Node2D
{
	[Export] PackedScene _overworldMenuScene;


	Node2D _loadedScene;
	OverworldPlayer _overworldPlayer;

	
	OverworldMenuController _overworldMenuController;

	GameState _gameState;

	[Signal] public delegate void SwitchToBattleEventHandler(EnemyEncounterResource enemyEncounterResource);

    public override void _Ready()
	{
		_overworldMenuController = GetNode<OverworldMenuController>("OverworldMenuController");
	}

	public void BindServices(GameState gameState)
	{
		_gameState = gameState;

		_overworldMenuController.BindServices(_gameState);
	}

	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			SetProcess(true);
			SetProcessInput(true);
			SetPhysicsProcess(true);
			Show();

		} else {
			SetProcess(false);
			SetProcessInput(false);
			SetPhysicsProcess(false);
			Hide();
		}
	}

	public void LoadScene(PackedScene sceneResource)
	{
		_loadedScene = sceneResource.Instantiate() as Node2D;
		AddChild(_loadedScene);
		OnNewRoomLoaded();
	}

	private void OnNewRoomLoaded()
	{
		_overworldPlayer = _loadedScene.GetNode<OverworldPlayer>("OverworldPlayer");
		_overworldPlayer.OverworldMenuRequested += OnOverworldMenuRequested;

		// 1. Story Progress

		// 2. Combat Zones
		Node2D combatZones = _loadedScene.GetNode<Node2D>("CombatZones");
		foreach (CombatZone combatZone in combatZones.GetChildren())
		{
			// Signal Hookup, don't need to do any disconnects
			combatZone.EncounterStart += OnEncounterStart;
		}

		// 3. Interactables

		// 4. Transitions
	}

	private void OnOverworldMenuRequested()
	{
		GetTree().Paused = true;

		_overworldMenuController.OpenMenu();
	}

	private void CloseOverworldMenu()
	{
		
	}

	private void OnEncounterStart(EnemyEncounterResource enemyEncounterResource)
	{
		_overworldPlayer.SetPhysicsProcess(false);
		EmitSignal(SignalName.SwitchToBattle, enemyEncounterResource);
	}

}
