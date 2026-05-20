using Godot;
using System;

public partial class FightingActor : CharacterBody2D
{
	public enum ActorAnimState
	{
		Idle,
		Injured,
		Dead,

		Walk,

		Takeoff,
		Jump,
		Land,

		

	}
}
