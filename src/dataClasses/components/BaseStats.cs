using Godot;
using System;

/*
*/
[GlobalClass]
public partial class BaseStats : Resource
{
    [Export] int _maxHP = 0;
	[Export] int _maxMP = 0;
	[Export] int _strength = 0;
	[Export] int _elemental = 0;
	[Export] int _agility = 0;
	[Export] int _luck = 0;
	[Export] int _defense = 0;
    [Export] int _resistance = 0;

	public BaseStats() : this(-1) {}
	public BaseStats(
		int maxHP
	)
	{
		_maxHP = maxHP;
	}

	public int GetMaxHP() { return _maxHP; }
	public int GetMaxMP() { return _maxMP; }

	public int GetStrength() { return _strength; }
	public int GetElemental() { return _elemental; }
	public int GetAgility() { return _agility; }
	public int GetLuck() { return _luck; }
	public int GetDefense() { return _defense; }
	public int GetResistance() { return _resistance; }
}
