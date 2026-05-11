using Godot;
using System;

public partial class BattleArena : Control
{
	[Export] PackedScene battleActorScene;

	[Signal] public delegate void BattleFinishedEventHandler(BattleController.BattleConclusion battleConclusion);

	Godot.Collections.Array<BattleActor> _actors = [];

	ActorController _actorController;
	UIMenuController _menuController;
	UITurnBar _turnBar;
	

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

		// UI
		_menuController = GetNode<UIMenuController>("UI/UIMenuController");
		_menuController.BindServices(_actorController, _actors);

		_turnBar = GetNode<UITurnBar>("UI/UITurnBar");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.E)
			{
				_actorController.AddActorCurHP(_actors[2], -5);
				GD.Print(_actors[2].GetCurHP());
			}
		}
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


	public void StartBattle()
	{
	}

	#region Signal Functions
	private void OnPartyMemberActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
		_menuController.PartyTurnStart(actor);
		
	}

	private void OnEnemyActorReady(BattleActor actor)
	{
		TimeScale = 0.0;
	}

	private void OnActionSelected()
	{
		
	}
	#endregion
}
