using Godot;
using System;

public partial class BattleSequencePlayer : Node2D
{
	[Signal] public delegate void BattleSequenceFinishedEventHandler();

	UseableActionResource _selectedAction;

	GameCamera _gameCamera;
	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;

	int _sequenceIndex = -1;

	public void BindServices(GameCamera gameCamera, MusicPlayer musicPlayer, SFXPlayer sfxPlayer)
	{
		_gameCamera = gameCamera;
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;
	}

	public void RunSequence(UseableActionResource selectedAction)
	{
		_selectedAction = selectedAction;
	}
}
