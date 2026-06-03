using Godot;
using System;

public partial class BattleArena : Control
{
	[Export] PackedScene battleActorScene;

	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	Godot.Collections.Array<BattleActor> _actors = [];
	Godot.Collections.Array<BattleActor> _partyMembers = [];
	Godot.Collections.Array<BattleActor> _enemyMembers = [];
	
	BattleActor _currentActor;
	BattleActor _partnerActor;

	Godot.Collections.Array<InventoryItem> _battleInventory = [];

	ActorController _actorController;
	BattleTriggerController _battleTriggerController;

	UIMenuController _menuController;
	UITurnBar _turnBar;
	UIBattleTextController _battleTextController;

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
		_actorController.EnemySkillUsed += OnSkillUsed;
		_actorController.RandomSelectAction += OnActionTargetConfimed;

		_battleTriggerController = GetNode<BattleTriggerController>("BattleTriggerController");
		_battleTriggerController.SideEffectsRequested += OnSideEffectRequested;


		// UI
		_menuController = GetNode<UIMenuController>("UI/UIMenuController");
		_menuController.BindServices(_actorController, _actors);
		
		_menuController.SkillUsed += OnSkillUsed;
		_menuController.FusionSkillUsed += OnFusionUsed;
		_menuController.ItemUsed += OnItemUsed;

		_menuController.ActionTargetConfirmed += OnActionTargetConfimed;

		_turnBar = GetNode<UITurnBar>("UI/UITurnBar");

		_battleTextController = GetNode<UIBattleTextController>("UI/UIBattleTextController");
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
				activePartyMember.GetPartyMemberFusionID(), 
				activePartyMember.GetCharacterStats(),
				activePartyMember.GetEquippedSkills(),
				activePartyMember.GetEquippedFusionSkills(),
				activePartyMember.GetCharacterAffinity(),
				activePartyMember.GetSpriteFrames(),
				activePartyMember.GetBattleIcon(),
				true
			);

			newActor.ReadyToAct += OnPartyMemberActorReady;

			_actors.Add(newActor);
			_partyMembers.Add(newActor);

			_battleTriggerController.CreateActorContainer(newActor);
		}

		// Enemies
		for (int i=0; i < enemies.Count; i++)
		{
			EnemyResource enemy = enemies[i];
			BattleActor newActor = battleActorScene.Instantiate() as BattleActor;
			_actorController.AddChild(newActor);
			
			CharacterStats enemyStats = new CharacterStats(enemy.GetBaseStats());
			CharacterAffinity enemyAffinity = new CharacterAffinity(enemy.GetBaseAffinity());

			newActor.Setup(
				70+(i % 2*35)+(i / 3*40),
				58+(i % 3*30),
				enemy.GetEnemyName(), 
				-1,
				enemyStats,
				enemy.GetSkills(),
				[],
				enemyAffinity,
				enemy.GetSpriteFrames(),
				enemy.GetBattleIcon(),
				false
			);

			newActor.ReadyToAct += OnEnemyActorReady;

			_actors.Add(newActor);
			_enemyMembers.Add(newActor);

			_battleTriggerController.CreateActorContainer(newActor);
		}

		_turnBar.Setup(_actors);	
		_battleTextController.Setup(_actors);
		IsActive = true;

		_actorController.Setup(_partyMembers, _enemyMembers);
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

		_battleTriggerController.RunActorSideEffects(_currentActor, BattleConsts.TriggerType.OnUserTurnStart);

		if (_currentActor.SelectRandomAction)
		{
			_actorController.SelectRandomAction(_currentActor);
		} else {
			_menuController.PartyTurnStart(_currentActor, _battleInventory);
		}
	}

	private void OnEnemyActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
		_currentActor = actor;

		_battleTriggerController.RunActorSideEffects(_currentActor, BattleConsts.TriggerType.OnUserTurnStart);

		if (_currentActor.SelectRandomAction)
		{
			_actorController.SelectRandomAction(_currentActor);
		} else {
			_actorController.EnemyAISelectAction(_currentActor);
		}
	}

	/*
		"Primary" actions go into this function.
	*/
	private void OnActionTargetConfimed(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedActors)
	{
		foreach(ActionEffectResource actionEffect in selectedAction.GetActions())
		{
			// Do not run if currentActor is not alive
			if (0 < _currentActor.GetCurHP())
			{
				// 1. Does the effect occur?
				if (actionEffect.GetSuccessChance() > GD.Randi() % 99 || _currentActor.SkillSuccessGuarantee)
				{
					// 2. Who do we target?
					if (selectedAction.GetTargetType() == BattleConsts.TargetType.Random)
					{
						int targetIndex = GD.RandRange(0, selectedActors.Count - 1);
						actionEffect.ExecuteEffect(_currentActor, selectedActors[targetIndex], _actorController);
					} else {
						foreach (BattleActor target in selectedActors)
						{
							actionEffect.ExecuteEffect(_currentActor, target, _actorController);
						}
					}
				}
			} else {
				// Stop action execution, the user is dead
				break;	
			}
		}
		OnActionFinished();
	}

	/*
		Secondary actions go into this function.
		These effects require no/minimal (non-waiting) animations.
	*/
	private void OnSideEffectRequested(Godot.Collections.Array<ActionEffectResource> actions, BattleActor battleActor)
	{
		GD.Print(battleActor.GetActorName());
		foreach(ActionEffectResource actionEffect in actions)
		{
			actionEffect.ExecuteEffect(_currentActor, battleActor, _actorController);
		}
	}

	private void OnActionFinished()
	{
		_battleTriggerController.RunActorSideEffects(_currentActor, BattleConsts.TriggerType.OnUserTurnEnd);


		// Turn Decrement
		_currentActor.TurnEnd();

		// Victory Check
		if (_actorController.GetLiveActors(_partyMembers).Count == 0)
		{
			IsActive = false;
			EmitSignal(SignalName.BattleFinished, (int)BattleController.BattleConclusion.Defeat);
		} else if (_actorController.GetLiveActors(_enemyMembers).Count == 0) {
			IsActive = false;
			EmitSignal(SignalName.BattleFinished, (int)BattleController.BattleConclusion.Victory);
		} else {
			// Required due to Godot's order of operations
			Callable.From(ResetActors).CallDeferred();
		}

		TimeScale = 1.0;
	}

	private void ResetActors()
	{
		_currentActor.ResetReadiness();
		_currentActor = null;

		if (_partnerActor != null)
		{
			_partnerActor.ResetReadiness();
			_partnerActor = null;
		}
	}

	/*
		Action Middleware
	*/

	private void OnSkillUsed(UseableSkillResource.SkillCostType skillCostType, int amount)
	{
		if (!_currentActor.IgnoreSkillCosts)
		{
			if (amount != 0)
			{
				if (skillCostType == UseableSkillResource.SkillCostType.HP)
				{
					_actorController.AddActorCurHP(_currentActor, -amount);
				} else {
					_actorController.AddActorCurMP(_currentActor, -amount);
				}
			}
		}
	}

	private void OnFusionUsed(int partnerID, UseableSkillResource.SkillCostType skillCostType, int amount)
	{
		_partnerActor = BattleConsts.FindActorByFusionID(partnerID, _partyMembers);
		if (!_currentActor.IgnoreSkillCosts)
		{
			if (amount != 0)
			{
				if (skillCostType == UseableSkillResource.SkillCostType.HP)
				{
					_actorController.AddActorCurHP(_currentActor, -amount);
				} else {
					_actorController.AddActorCurMP(_currentActor, -amount);
				}
			}
		}

		if (!_partnerActor.IgnoreSkillCosts)
		{
			if (amount != 0)
			{
				if (skillCostType == UseableSkillResource.SkillCostType.HP)
				{
					_actorController.AddActorCurHP(_partnerActor, -amount);
				} else {
					_actorController.AddActorCurMP(_partnerActor, -amount);
				}
			}
		}
	}

	private void OnItemUsed(int itemIndex, int quantity)
	{
		_inventoryController.AddItemQuantity(_battleInventory, itemIndex, -quantity);
	}
	#endregion
}
