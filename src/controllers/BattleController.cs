using Godot;
using System;

public partial class BattleController : CanvasLayer
{
	[Export] PackedScene battleArenaScene;


	public enum BattleConclusion
	{
		Victory,
		Defeat,
		Escape,
		Interrupt
	}

	EnemyEncounterResource _enemyEncounterResource;

	InventoryController _inventoryController;
	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;

	public void BindServices(InventoryController inventoryController, MusicPlayer musicPlayer, SFXPlayer sfxPlayer)
	{
		_inventoryController = inventoryController;
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

	public void SetupBattle(Godot.Collections.Array<ActivePartyMember> partyMembers, EnemyEncounterResource enemyEncounterResource)
	{
		_enemyEncounterResource = enemyEncounterResource;

		BattleArena newBattleArena = battleArenaScene.Instantiate() as BattleArena;
		AddChild(newBattleArena);

		newBattleArena.BattleFinished += OnBattleFinished;
		
		newBattleArena.SetupActors(partyMembers, enemyEncounterResource.GetEnemies());

		if (enemyEncounterResource.GetBattleMusic() != null)
		{
		}
	}

	private void OnBattleFinished(BattleConclusion battleConclusion)
	{
		switch(battleConclusion)
		{
			case BattleConclusion.Victory:
			{
				break;
			}

			case BattleConclusion.Defeat:
			{
				break;
			}

			case BattleConclusion.Escape:
			{
				break;
			}

			case BattleConclusion.Interrupt:
			{
				break;
			}
		}
	}
}
