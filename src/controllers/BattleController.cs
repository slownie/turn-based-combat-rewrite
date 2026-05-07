using Godot;
using System;

public partial class BattleController : CanvasLayer
{
	[Export] PackedScene battleArenaScene;

	EnemyEncounterResource _enemyEncounterResource;

	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;

	public void BindServices(MusicPlayer musicPlayer, SFXPlayer sfxPlayer)
	{
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;
	}

	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			SetProcess(true);
			SetProcessInput(true);
			Show();
		} else {
			SetProcess(false);
			SetProcessInput(false);
			Hide();
		}
	}

	public void SetupBattle(EnemyEncounterResource enemyEncounterResource)
	{
		_enemyEncounterResource = enemyEncounterResource;

		BattleArena newBattleArena = battleArenaScene.Instantiate() as BattleArena;
		AddChild(newBattleArena);

		newBattleArena.BattleFinished += OnBattleFinished;
	}

	private void OnBattleFinished(BattleArena.BattleOutcome outcome)
	{
		switch (outcome)
		{
			case BattleArena.BattleOutcome.Victory:
			{
				break;
			}

			case BattleArena.BattleOutcome.Defeat:
			{
				break;
			}

			case BattleArena.BattleOutcome.Escape:
			{
				break;
			}

			case BattleArena.BattleOutcome.Interrupt:
			{
				break;
			}
		}
	}
}
