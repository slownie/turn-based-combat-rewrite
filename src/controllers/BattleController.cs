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
	GameCamera _gameCamera;

	public void BindServices(
		InventoryController inventoryController, 
		MusicPlayer musicPlayer, 
		SFXPlayer sfxPlayer,
		GameCamera gameCamera
	)
	{
		_inventoryController = inventoryController;
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;
		_gameCamera = gameCamera;
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

	public void SetupBattle(
		Godot.Collections.Array<ActivePartyMember> partyMembers, 
		EnemyEncounterResource enemyEncounterResource,
		Godot.Collections.Array<InventoryItem> inventoryItems
	)
	{
		_enemyEncounterResource = enemyEncounterResource;

		BattleArena newBattleArena = battleArenaScene.Instantiate() as BattleArena;
		AddChild(newBattleArena);

		newBattleArena.BattleFinished += OnBattleFinished;
		
		newBattleArena.SetupActors(partyMembers, enemyEncounterResource.GetEnemies());
		newBattleArena.SetupInventory(inventoryItems);
		newBattleArena.BindServices(_inventoryController, _musicPlayer, _sfxPlayer, _gameCamera);

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
				GD.Print("You Win!");
				break;
			}

			case BattleConclusion.Defeat:
			{
				GD.Print("You Lose!");
				break;
			}

			case BattleConclusion.Escape:
			{
				GD.Print("Escaped!");
				break;
			}

			case BattleConclusion.Interrupt:
			{
				break;
			}
		}
	}
}
