using Godot;
using System;

public partial class Main : Node
{
	OverworldController _overworldController;
	BattleController _battleController;
	MenuController _menuController;

	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;
	ScreenTransition _screenTransition;

	GameState _gameState;

	// FOR TESTING PURPOSES ONLY
	[Export] Godot.Collections.Array<PartyMemberDataResource> startingPartyMembers = [];

	public override void _Ready()
	{
		BuildServices();

		GameInit();
	}

	private void BuildServices()
	{
		// Overworld Controller
		PackedScene overworldControllerScene = GD.Load<PackedScene>("res://src/controllers/OverworldController.tscn");
		_overworldController = overworldControllerScene.Instantiate() as OverworldController;
		AddChild(_overworldController);

		PackedScene initialOverworldScene = GD.Load<PackedScene>("res://scenes/overworld/TestOverworld.tscn");
		_overworldController.LoadScene(initialOverworldScene);

		_overworldController.SwitchToBattle += BattleStart;

		


		// Battle Controller
		PackedScene battleControllerScene = GD.Load<PackedScene>("res://src/controllers/BattleController.tscn");
		_battleController = battleControllerScene.Instantiate() as BattleController;
		AddChild(_battleController);

		_battleController.SetActive(false);

		// Menu Controller
		PackedScene menuControllerScene = GD.Load<PackedScene>("res://src/controllers/MenuController.tscn");
		_menuController = menuControllerScene.Instantiate() as MenuController;
		AddChild(_menuController);

		_menuController.SetActive(false);

		PackedScene musicPlayerScene = GD.Load<PackedScene>("res://src/music/MusicPlayer.tscn");
		_musicPlayer = musicPlayerScene.Instantiate() as MusicPlayer;
		AddChild(_musicPlayer);

		PackedScene sfxPlayerScene = GD.Load<PackedScene>("res://src/sfx/SFXPlayer.tscn");
		_sfxPlayer = sfxPlayerScene.Instantiate() as SFXPlayer;
		AddChild(_sfxPlayer);

		PackedScene screenTransitionScene = GD.Load<PackedScene>("res://src/transitions/ScreenTransition.tscn");
		_screenTransition = screenTransitionScene.Instantiate() as ScreenTransition;
		AddChild(_screenTransition);

		

		_gameState = new GameState();
		

		// TODO: REMOVE WHEN YOU ACTUALLY GET A WORKING MENU
		_gameState.NewGame(startingPartyMembers);
	}

	private void BattleStart(EnemyEncounterResource enemyEncounterResource)
	{
		GD.Print("Battle Start");
		_overworldController.SetActive(false);

		_battleController.SetActive(true);
		_battleController.SetupBattle(_gameState.GetActivePartyMembers(), enemyEncounterResource);
	}

	private void GameInit()
	{
		
	}
}
