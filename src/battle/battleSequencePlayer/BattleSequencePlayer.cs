using Godot;
using System;

public partial class BattleSequencePlayer : Node2D
{
	[Signal] public delegate void ExecuteActionEventHandler();
	[Signal] public delegate void BattleSequenceFinishedEventHandler();

	[Export] PackedScene hitEffectScene;

	Timer _timer;

	UseableActionResource _selectedAction;
	Godot.Collections.Array<BattleSequenceEffectResource> _sequence;

	GameCamera _gameCamera;
	MusicPlayer _musicPlayer;
	SFXPlayer _sfxPlayer;

	Marker2D _centerPosition;

	int _signalCounter = 0;

    public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
	}

	public void BindServices(GameCamera gameCamera, MusicPlayer musicPlayer, SFXPlayer sfxPlayer, Marker2D centerPosition)
	{
		_gameCamera = gameCamera;
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;

		_centerPosition = centerPosition;
	}

	public async void RunSequence(UseableActionResource selectedAction, BattleActor user, Godot.Collections.Array<BattleActor> targets)
	{
		_sequence = selectedAction.GetBattleSequence().Duplicate();
		while (_sequence.Count != 0)
		{
			BattleSequenceEffectResource currentEffect = _sequence[0];
			switch (currentEffect)
			{
				case PlayHitEffect hitEffect:
				{
					SignalGroup signalGroup = new SignalGroup();
					Godot.Collections.Array hitEffects = [];				


					foreach (BattleActor target in targets)
					{
						HitEffect sceneHitEffect = hitEffectScene.Instantiate() as HitEffect;
						AddChild(sceneHitEffect);
						hitEffects.Add(sceneHitEffect);

						sceneHitEffect.Setup(target.Position, hitEffect.GetSpriteFrames());
					}

					signalGroup.StartAwait(hitEffects, "HitEffectFinished");

						
					await ToSignal(signalGroup, SignalGroup.SignalName.SignalsCompleted);
					
					break;
				}

				case DoAction doAction:
				{
					EmitSignal(SignalName.ExecuteAction);
					break;
				}

				case MoveToPositionEffect moveToPosition:
				{
					Vector2 targetPosition = user.Position;

					// Invert offset for enemies
					Vector2 positionOffset = moveToPosition.GetPositionOffset();
					if (!user.GetIsPlayer()) positionOffset.X = positionOffset.X * -1;

					user.SetFlipX(moveToPosition.GetFlipXOnStart());

					// Determine targetPosition
					switch(moveToPosition.GetBattleArenaPosition())
					{
						case MoveToPositionEffect.BattleArenaPosition.None:
						{
							targetPosition = user.Position;
							break;
						}

						case MoveToPositionEffect.BattleArenaPosition.Initial:
						{
							targetPosition = user.GetInitialPosition();
							break;
						}

						case MoveToPositionEffect.BattleArenaPosition.Target:
						{
							targetPosition = targets[0].GetPosition() + positionOffset;
							break;
						}

						case MoveToPositionEffect.BattleArenaPosition.Center:
						{
							targetPosition = _centerPosition.Position;
							break;
						}

						case MoveToPositionEffect.BattleArenaPosition.Custom:
						{
							targetPosition = moveToPosition.GetCustomPosition();
							break;
						}
					}

					Tween tween = user.GetTree().CreateTween();
					tween.TweenProperty(user, "position", targetPosition, moveToPosition.GetMoveSpeed());
					await ToSignal(tween, "finished");

					user.SetFlipX(moveToPosition.GetFlipXOnEnd());

					break;
				}

				case ActorPlayAnimation actorPlayAnimation:
				{
					string animationName = actorPlayAnimation.GetAnimationName();
					if (actorPlayAnimation.GetPlayForUser())
					{
						user.SetAnimation(animationName);
					} else {
						foreach (BattleActor target in targets)
						{
							target.SetAnimation(animationName);
						}
					}
					break;
				}

				case WaitEffect waitEffect:
				{
					double seconds = waitEffect.GetSeconds();
					_timer.WaitTime = seconds;
					_timer.Start();
					await ToSignal(_timer, Timer.SignalName.Timeout);

					break;
				}
			}
			_sequence.RemoveAt(0);
		}

		EmitSignal(SignalName.BattleSequenceFinished);
	}
}
