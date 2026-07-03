using Godot;
using System;

[GlobalClass]
public partial class PlayHitEffect : BattleSequenceEffectResource
{
	[Export] SpriteFrames spriteFrames;

	public SpriteFrames GetSpriteFrames() { return spriteFrames; }
}
