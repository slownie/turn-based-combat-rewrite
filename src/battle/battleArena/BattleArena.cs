using Godot;
using System;

public partial class BattleArena : Control
{
	[Export] PackedScene battleActorScene;

	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	Godot.Collections.Array<BattleActor> _actors = [];
	BattleActor _currentActor;
	Godot.Collections.Array<InventoryItem> _battleInventory = [];

	ActorController _actorController;
	UIMenuController _menuController;
	UITurnBar _turnBar;

	InventoryController _inventoryController;
	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;
	GameCamera _gameCamera;
	
	// UI Objects

	/*
		Allows for pausing the Active Time Battler during combat intro, a cutscene, or combat end
	*/
	bool isActive = false;
	public bool IsActive
	{
		get { return isActive; }
		set
		{
			if (value != isActive)
			{
				isActive = value;
				foreach (BattleActor actor in _actors)
				{
					actor.IsActive = isActive;
				}
			}
		}
	}

	/*
		Multiplier for the global pace of battle.
		Stops when a battler is selecting/executing an action.
	*/
	double timeScale = 1.0;
	public double TimeScale
	{
		get { return timeScale; }
		set
		{
			timeScale = value;
			foreach(BattleActor actor in _actors)
			{
				actor.TimeScale = timeScale;
			}
		}
	}

	public override void _Ready()
	{
		_actorController = GetNode<ActorController>("ActorController");
		_actorController.EnemySelectAction += OnActionTargetConfimed;

		// UI
		_menuController = GetNode<UIMenuController>("UI/UIMenuController");
		_menuController.BindServices(_actorController, _actors);
		
		_menuController.SkillUsed += OnSkillUsed;
		_menuController.ItemUsed += OnItemUsed;

		_menuController.ActionTargetConfirmed += OnActionTargetConfimed;

		_turnBar = GetNode<UITurnBar>("UI/UITurnBar");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
		}
	}

	public void BindServices(InventoryController inventoryController, MusicPlayer musicPlayer, SFXPlayer sfxPlayer, GameCamera gameCamera)
	{
		_inventoryController = inventoryController;
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;
		_gameCamera = gameCamera;
	}

	public void SetupActors(Godot.Collections.Array<ActivePartyMember> partyMembers, Godot.Collections.Array<EnemyResource> enemies)
	{
		// Party Members		
		for (int i=0; i < partyMembers.Count; i++)
		{
			ActivePartyMember activePartyMember = partyMembers[i];
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
			
			newActor.Setup(
				250+(i % 3*10)+(i / 3*25),
				68+(i % 5*55),
				activePartyMember.GetPartyMemberName(), 
				activePartyMember.GetCharacterStats(),
				activePartyMember.GetEquippedSkills(),
				activePartyMember.GetSpriteFrames(),
				activePartyMember.GetBattleIcon(),
				true
			);

			newActor.ReadyToAct += OnPartyMemberActorReady;

			_actors.Add(newActor);
		}

		// Enemies
		for (int i=0; i < enemies.Count; i++)
		{
			EnemyResource enemy = enemies[i];
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
			
			CharacterStats enemyStats = new CharacterStats(enemy.GetBaseStats());

			newActor.Setup(
				70+(i % 2*35)+(i / 3*40),
				58+(i % 3*30),
				enemy.GetEnemyName(), 
				enemyStats,
				enemy.GetSkills(),
				enemy.GetSpriteFrames(),
				enemy.GetBattleIcon(),
				false
			);

			newActor.ReadyToAct += OnEnemyActorReady;

			_actors.Add(newActor);
		}

		_turnBar.Setup(_actors);	
		IsActive = true;
	}

	public void SetupInventory(Godot.Collections.Array<InventoryItem> inventory)
	{
		_battleInventory = inventory;
	}

	public void StartBattle()
	{
	}

	#region Signal Functions
	private void OnPartyMemberActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
		_currentActor = actor;
		_menuController.PartyTurnStart(actor, _battleInventory);
	}

	private void OnEnemyActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
		_currentActor = actor;
		_actorController.EnemyAISelectAction(_currentActor, _actors);
	}

	private void OnActionTargetConfimed(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedActors)
	{
		GD.Print("==OnActionTargetConfirmed===");
		// Start the action
		foreach(ActionEffectResource actionEffect in selectedAction.GetActions())
		{
			// 1. Does the effect occur?
			if (actionEffect.GetSuccessChance() > GD.Randi() % 99)
			{
				// 2. Who do we target?
				if (selectedAction.GetTargetType() == BattleConsts.TargetType.Random)
				{
					int targetIndex = GD.RandRange(0, selectedActors.Count - 1);
					actionEffect.ExecuteEffect(_currentActor, selectedActors[targetIndex]);
				} else {
					foreach (BattleActor target in selectedActors)
					{
						actionEffect.ExecuteEffect(_currentActor, target);
					}
				}
			}
		}
		TimeScale = 1.0;
	}

	private void OnActionFinished()
	{
		_currentActor = null;
	}

	/*
		Action Middleware
	*/

	private void OnSkillUsed(BattleActor actor, UseableSkillResource.SkillCostType skillCostType, int amount)
	{
		if (skillCostType == UseableSkillResource.SkillCostType.HP)
		{
			_actorController.AddActorCurHP(actor, -amount);
		} else {
			_actorController.AddActorCurMP(actor, -amount);
			GD.Print(actor.GetCurMP());
		}
	}

	private void OnItemUsed(int itemIndex, int quantity)
	{
		_inventoryController.AddItemQuantity(_battleInventory, itemIndex, -quantity);
	}
	#endregion
}
