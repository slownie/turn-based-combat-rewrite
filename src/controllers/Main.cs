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

	}

	private void BattleStart(EnemyEncounterResource enemyEncounterResource)
	{
		_screenTransition.FadeOutTransition();
		_overworldController.SetActive(false);

		_battleController.SetActive(true);
		_battleController.SetupBattle(enemyEncounterResource);
		_screenTransition.FadeInTransition();
	}

	private void GameInit()
	{
		
	}
}
