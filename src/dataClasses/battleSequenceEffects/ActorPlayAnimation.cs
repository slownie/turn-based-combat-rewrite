using Godot;
using System;

[GlobalClass]
public partial class ActorPlayAnimation : BattleSequenceEffectResource
{
	[Export] string animationName;
	[Export] bool playForUser = false;
	[Export] bool waitToFinish = true;
	
	public string GetAnimationName() { return animationName; }
	public bool GetPlayForUser() { return playForUser; }
	public bool GetWaitToFinish() { return waitToFinish; }
}
