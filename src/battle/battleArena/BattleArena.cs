using Godot;
using System;

public partial class BattleArena : Control
{
	[Export] PackedScene battleActorScene;
	[Export] PackedScene followerActorScene;

	[ExportCategory("UI")]
	[Export] PackedScene partyEntryScene;

	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	enum DefensiveMode { Hit, Guard, Knockback, Backdash }
	DefensiveMode _currentDefensiveMode = DefensiveMode.Hit;

	Godot.Collections.Array<BattleActor> _actors = [];
	Godot.Collections.Array<BattleActor> _partyMembers = [];
	Godot.Collections.Array<BattleActor> _enemyMembers = [];
	Godot.Collections.Array<FollowerActor> _enemyfollowerActors = [];
	
	BattleActor _currentActor;

	// Secondary Actors
	BattleActor _partnerActor;
	FollowerActor _followerActor;

	Godot.Collections.Array<BattleActor> _selectedTargets = [];
	UseableActionResource _selectedAction;

	Godot.Collections.Array<InventoryItem> _battleInventory = [];

	ActorController _actorController;
	BattleTriggerController _battleTriggerController;

	UIMenuController _menuController;
	UITurnBar _turnBar;
	UIBattleTextController _battleTextController;
	UIPartyEntries _partyEntries;

	InventoryController _inventoryController;
	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;
	GameCamera _gameCamera;



	Timer _defenseTimer;
	Label _timerLabel; // TESTING PURPOSES ONLY
	
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

				foreach (FollowerActor followerActor in _enemyfollowerActors)
				{
					followerActor.IsActive = isActive;
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

			foreach (FollowerActor followerActor in _enemyfollowerActors)
			{
				followerActor.TimeScale = timeScale;
			}
		}
	}

	bool _canEscape = true;
	bool _didEscape = false;

	public override void _Ready()
	{
		_actorController = GetNode<ActorController>("ActorController");
		_actorController.EnemySelectAction += OnActionTargetConfimed;
		_actorController.EnemySkillUsed += OnSkillUsed;
		_actorController.RandomSelectAction += OnActionTargetConfimed;
		_actorController.EscapeSuccess += OnEscapeSuccess;

		_battleTriggerController = GetNode<BattleTriggerController>("BattleTriggerController");
		_battleTriggerController.SideEffectsRequested += OnSideEffectRequested;

		_defenseTimer = GetNode<Timer>("DefenseTimer");
		_timerLabel = GetNode<Label>("UI/TimerLabel");

		// UI
		_menuController = GetNode<UIMenuController>("UI/UIMenuController");
		_menuController.BindServices(_actorController, _actors);
		
		_menuController.SkillUsed += OnSkillUsed;
		_menuController.FusionSkillUsed += OnFusionUsed;
		_menuController.ItemUsed += OnItemUsed;

		_menuController.ActionTargetConfirmed += OnActionTargetConfimed;

		_partyEntries = GetNode<UIPartyEntries>("UI/UIPartyEntries");

		_turnBar = GetNode<UITurnBar>("UI/UITurnBar");

		_battleTextController = GetNode<UIBattleTextController>("UI/UIBattleTextController");
	}

    public override void _Process(double delta)
	{
		// TESTING CODE ONLY
		if (0 < _defenseTimer.TimeLeft)
		{
			_timerLabel.Text = Math.Round(_defenseTimer.TimeLeft, 2).ToString();
		}
	}

    public override void _Input(InputEvent @event)
	{
		// Defensive Actions
		if (0 < _defenseTimer.TimeLeft)
		{
			if (@event.IsActionPressed("AButton"))
			{
				_currentDefensiveMode = DefensiveMode.Guard;
				GD.Print("Guard");
			}

			if (@event.IsActionPressed("BButton"))
			{
				_currentDefensiveMode = DefensiveMode.Hit;
				GD.Print("Hit");
			}

			if (@event.IsActionPressed("YButton"))
			{
				_currentDefensiveMode = DefensiveMode.Backdash;
				GD.Print("Backdash");
			}

			if (@event.IsActionPressed("XButton"))
			{
				_currentDefensiveMode = DefensiveMode.Knockback;
				GD.Print("Knockback");
			}
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

			newActor.ReadyToAct += OnActorReady;

			_actors.Add(newActor);
			_partyMembers.Add(newActor);

			_battleTriggerController.CreateActorContainer(newActor);
			// Execution order
			newActor.ConnectPassiveSkills();
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

			newActor.ReadyToAct += OnActorReady;

			_actors.Add(newActor);
			_enemyMembers.Add(newActor);

			// Create Followers
			if (0 < enemy.GetFollowerActorAmount())
			{
				int initialReadiness = 20;
				for (int j=0; j < enemy.GetFollowerActorAmount(); j++)
				{
					FollowerActor newFollower = followerActorScene.Instantiate() as FollowerActor;
					_actorController.AddChild(newFollower);

					newFollower.Setup(newActor);
					newFollower.ReadyToAct += OnFollowerReady;

					_enemyfollowerActors.Add(newFollower);
					newFollower.AddReadiness(initialReadiness + (initialReadiness * j));
				}
			}

			_battleTriggerController.CreateActorContainer(newActor);
			newActor.ConnectPassiveSkills();
		}

		_turnBar.Setup(_actors, _enemyfollowerActors);	
		_partyEntries.Setup(_partyMembers);
		_battleTextController.Setup(_actors);

		_actorController.Setup(_partyMembers, _enemyMembers);

		StartBattle();
	}

	public void SetupInventory(Godot.Collections.Array<InventoryItem> inventory)
	{
		_battleInventory = inventory;
	}

	public void StartBattle()
	{
		foreach(BattleActor actor in _actors)
		{
			_battleTriggerController.RunActorSideEffects(actor, BattleConsts.TriggerType.OnBattleStart);
		}

		IsActive = true;
	}

	private void ExecuteActionEffects()
	{
		foreach(ActionEffectResource actionEffect in _selectedAction.GetActions())
		{
			// Do not run if currentActor is not alive
			if (0 < _currentActor.GetCurHP())
			{
				// 1. Does the effect occur?
				if (actionEffect.GetSuccessChance() > GD.Randi() % 99 || _currentActor.SkillSuccessGuarantee)
				{
					// 2. Who do we target?
					if (_selectedAction.GetTargetType() == BattleConsts.TargetType.Random)
					{
						int targetIndex = GD.RandRange(0, _selectedTargets.Count - 1);
						actionEffect.ExecuteEffect(_currentActor, _selectedTargets[targetIndex], _actorController);
					} else {
						foreach (BattleActor target in _selectedTargets)
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



	#region Signal Functions
	private void OnActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
		_currentActor = actor;

		_battleTriggerController.RunActorSideEffects(_currentActor, BattleConsts.TriggerType.OnUserTurnStart);

		if (_currentActor.SelectRandomAction)
		{
			_actorController.SelectRandomAction(_currentActor);
		} else if (_currentActor.GetQueueAction() != null) {
			_actorController.SelectSetAction(_currentActor, _currentActor.GetQueueAction());
		} else {
			if (_currentActor.GetIsPlayer())
			{
				_menuController.PartyTurnStart(_currentActor, _battleInventory);
			} else {
				_actorController.EnemyAISelectAction(_currentActor);
			}
		}
	}

	private void OnFollowerReady(FollowerActor followerActor)
	{
		TimeScale = 0.0;
		_followerActor = followerActor;

		OnActorReady(followerActor.GetLeaderActor());
	}

	/*
		"Primary" actions go into this function.
	*/
	private async void OnActionTargetConfimed(UseableActionResource selectedAction, Godot.Collections.Array<BattleActor> selectedTargets)
	{
		_selectedAction = selectedAction;
		_selectedTargets = selectedTargets;

		/*
		if (!_currentActor.GetIsPlayer())
		{
			_defenseTimer.Start();
			await ToSignal(_defenseTimer, Timer.SignalName.Timeout);
		}
		*/

		ExecuteActionEffects();
	}

	/*
		Secondary actions go into this function.
		These effects require no/minimal (non-waiting) animations.
	*/
	private void OnSideEffectRequested(ActivePassiveEffect activePassiveEffect, BattleTriggerController.EffectDirective effectDirective, BattleActor battleActor)
	{
		// Could compress this using a dictionary but I'm not in the mood for refactoring 
		switch (effectDirective)
		{
			case BattleTriggerController.EffectDirective.Startup:
			{
				foreach(ActionEffectResource actionEffect in activePassiveEffect.GetStartupEffects())
				{
					actionEffect.ExecuteEffect(_currentActor, battleActor, _actorController);
				}
				break;
			}

			case BattleTriggerController.EffectDirective.Trigger:
			{
				foreach(ActionEffectResource actionEffect in activePassiveEffect.GetTriggerEffects())
				{
					actionEffect.ExecuteEffect(_currentActor, battleActor, _actorController);
				}
				break;
			}

			case BattleTriggerController.EffectDirective.Cleanup:
			{
				foreach(ActionEffectResource actionEffect in activePassiveEffect.GetCleanupEffects())
				{
					actionEffect.ExecuteEffect(_currentActor, battleActor, _actorController);
				}
				break;
			}
		}
	}

	private void OnActionFinished()
	{
		// Reset Variables
		_selectedAction = null;
		_selectedTargets = [];

		// Turn Decrement
		if (!_didEscape) {
			_battleTriggerController.RunActorSideEffects(_currentActor, BattleConsts.TriggerType.OnUserTurnEnd);
			_currentActor.TurnEnd();
		}
		// Victory Check
		if (_actorController.GetLiveActors(_partyMembers).Count == 0)
		{
			IsActive = false;
			EmitSignal(SignalName.BattleFinished, (int)BattleController.BattleConclusion.Defeat);
		} else if (_actorController.GetLiveActors(_enemyMembers).Count == 0) {
			IsActive = false;
			EmitSignal(SignalName.BattleFinished, (int)BattleController.BattleConclusion.Victory);
		} else if (_didEscape) {
			IsActive = false;
			EmitSignal(SignalName.BattleFinished, (int)BattleController.BattleConclusion.Escape);
		} else {
			// Required due to Godot's order of operations
			Callable.From(ResetActors).CallDeferred();
		}

		TimeScale = 1.0;
	}

	private void ResetActors()
	{
		if (_followerActor != null)
		{
			// Is the follower ready?
			_followerActor.ResetReadiness();
			_followerActor = null;			
		} else {
			_currentActor.ResetReadiness();
		}
		_currentActor = null;

		if (_partnerActor != null)
		{
			_partnerActor.ResetReadiness();
			_partnerActor = null;
		}
	}

	private void OnEscapeSuccess()
	{
		_didEscape = true;
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
