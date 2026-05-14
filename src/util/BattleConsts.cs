using Godot;
using System;

public static partial class BattleConsts
{
	public enum DamageType
	{
		Physical,
		Elemental
	}

	public enum ElementType
	{
		Phys,
		Fire,
		Water,
		Wind,
		Earth,
		Steam,
		Electric,
		Metal,
		Ice,
		Life,
		Gravity
	}

	/*
		When the targets array is passed into the skill's function, the target type will
		determine which target the effect is applied to.
			- Single: Only one target, the targets array will already only have the one target in it.
			- All: Applies to all targets in the targets array.
			- Random: Randomly selects a target from the targets array.
	*/
	public enum TargetType
	{
		Single,
		All,
		Random,
	}

	/*
		Determines the control and quantity of cursors when selecting targets.
	*/
	public enum CursorMode
	{
		Single,
		Side,
		All,
	}

	/*
		Used to as a middleware between target selection and action execution.
		Could probably be coded to look for children nodes but I think this works better.
	*/
	public enum ActionMenuType
	{
		Attack,
		Skill,
		Item,
		Pass,
		Other
	}
}
