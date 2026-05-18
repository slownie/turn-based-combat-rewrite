using Godot;
using System;

public partial class SFXPlayer : AudioStreamPlayer2D
{
	public void PlaySFX(AudioStream soundEffect)
	{
		Stream = soundEffect;
		Play();
	}
}
