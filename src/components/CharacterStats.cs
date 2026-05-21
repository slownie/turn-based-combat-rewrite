using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : GodotObject
{
	[Signal] public delegate void HPChangedEventHandler();

	[Signal] public delegate void HPDepletedEventHandler();

	int _maxHP = 0;
	int _maxMP = 0;
	int _curHP = 0;
	int _curMP = 0;

	int _strength = 0;
	int _elemental = 0;
	int _agility = 0;
	int _luck = 0;
	int _defense = 0;
    int _resistance = 0;

	public CharacterStats() : this(null,-1,-1) {}
	public CharacterStats(BaseStats baseStats, int curHP=-1,int curMP=-1)
	{
		_maxHP = baseStats.GetMaxHP();
		_maxMP = baseStats.GetMaxMP();
		_strength = baseStats.GetStrength();
		_elemental = baseStats.GetElemental();
		_agility = baseStats.GetAgility();
		_luck = baseStats.GetLuck();
		_defense = baseStats.GetDefense();
		_resistance = baseStats.GetResistance();

		if (curHP == -1)
		{
			_curHP = _maxHP;
		} else {
			_curHP = curHP;
		}

		if (curMP == -1)
		{
			_curMP = _maxMP;
		} else {
			_curMP = curMP;
		}
	}

	public int GetMaxHP() { return _maxHP; }
	public int GetMaxMP() { return _maxMP; }
	public int GetCurHP() { return _curHP; }
	public int GetCurMP() { return _curMP; }

	public int GetStrength() { return _strength; }
	public int GetElemental() { return _elemental; }
	public int GetAgility() { return _agility; }
	public int GetLuck() { return _luck; }
	public int GetDefense() { return _defense; }
	public int GetResistance() { return _resistance; }

	public void AddCurHP(int amount)
	{
		_curHP += amount;

		if (_curHP < 0) {
			_curHP = 0;
			EmitSignal(SignalName.HPDepleted);
		}
		if (_maxHP < _curHP) _curHP = _maxHP;
	}

	public void SetCurHP(int amount)
	{
		_curHP = amount;
		if (_curHP < 0) _curHP = 0;
		if (_maxHP < _curHP) _curHP = _maxHP;
	}

	public void AddCurMP(int amount)
	{
		_curMP += amount;
		if (_curMP < 0) _curMP = 0;
		if (_maxMP < _curMP) _curMP = _maxMP;
	}

	public void SetCurMP(int amount)
	{
		_curMP = amount;
		if (_curMP < 0) _curMP = 0;
		if (_maxMP < _curMP) _curMP = _maxMP;
	}
}
