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

	int _signalCounter = 0;

    public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
	}

	public void BindServices(GameCamera gameCamera, MusicPlayer musicPlayer, SFXPlayer sfxPlayer)
	{
		_gameCamera = gameCamera;
		_musicPlayer = musicPlayer;
		_sfxPlayer = sfxPlayer;
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
					Godot.Collections.Array<HitEffect> hitEffects = [];				


					foreach (BattleActor target in targets)
					{
						HitEffect sceneHitEffect = hitEffectScene.Instantiate() as HitEffect;
						AddChild(sceneHitEffect);
						hitEffects.Add(sceneHitEffect);

						sceneHitEffect.Setup(target.Position, hitEffect.GetSpriteFrames());
					}

					signalGroup.StartAwait(hitEffects);

						
					await ToSignal(signalGroup, SignalGroup.SignalName.SignalsCompleted);
					
					break;
				}

				case DoAction doAction:
				{
					EmitSignal(SignalName.ExecuteAction);
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
