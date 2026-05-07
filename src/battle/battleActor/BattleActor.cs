using Godot;
using System;

public partial class BattleActor : Node2D
{
	string _name = "Placeholder";

	int _maxHP = 1;
	int _maxMP = 1;
	int _strength = 1;
	int _elemental = 1;
	int _agility = 1;
	int _luck = 1;
	int _defense = 1;
	int _resistance = 1;

	int _curHP = 1;
	int _curMP = 1; 


	public BattleActor(string name,
		int maxHP,
		int maxMP,
		int strength,
		int elemental,
		int agility,
		int luck,
		int defense,
		int resistance
	)
	{
		_name = name;
		
		_maxHP = maxHP;
		_maxMP = maxMP;
		_strength = strength;
		_elemental = elemental;
		_agility = agility;
		_luck = luck;
		_defense = defense;
		_resistance = resistance;

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
