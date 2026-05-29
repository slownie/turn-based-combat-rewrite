using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : GodotObject
{
	[Signal] public delegate void HPChangedEventHandler();

	[Signal] public delegate void HPReviveEventHandler();
	[Signal] public delegate void HPDepletedEventHandler();

	int _maxHP = 0;
	int _maxMP = 0;
	int _curHP = 0;
	int curHP
	{
		get { return _curHP; }
		set
		{
			// Did this revive the actor?
			if (_curHP == 0 && 0 < value)
			{
				EmitSignal(SignalName.HPRevive);
			}

			_curHP = value;

			// Did this kill the actor?
			if (_curHP <= 0)
			{
				_curHP = 0;
				EmitSignal(SignalName.HPDepleted);
			}

			if (_maxHP < _curHP) _curHP = _maxHP;
		}
	}

	int _curMP = 0;
	int curMP
	{
		get { return _curMP; }
		set
		{
			_curMP = value;

			if (_curHP < 0) { _curMP = 0; }
			if (_maxMP < _curMP) { _curMP = _maxMP; }
		}
	}

	int _strength = 0;
	int _elemental = 0;
	int _agility = 0;
	int _luck = 0;
	int _defense = 0;
    int _resistance = 0;

	public CharacterStats() : this(null,-1,-1) {}
	public CharacterStats(BaseStats baseStats, int _curHP=-1,int _curMP=-1)
	{
		_maxHP = baseStats.GetMaxHP();
		_maxMP = baseStats.GetMaxMP();
		_strength = baseStats.GetStrength();
		_elemental = baseStats.GetElemental();
		_agility = baseStats.GetAgility();
		_luck = baseStats.GetLuck();
		_defense = baseStats.GetDefense();
		_resistance = baseStats.GetResistance();

		if (_curHP == -1)
		{
			curHP = _maxHP;
		} else {
			curHP = _curHP;
		}

		if (_curMP == -1)
		{
			curMP = _maxMP;
		} else {
			curMP = _curMP;
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
		curHP += amount;
	}

	public void SetCurHP(int amount)
	{
		curHP = amount;
	}

	public void AddCurMP(int amount)
	{
		_curMP += amount;
	}

	public void SetCurMP(int amount)
	{
		_curMP = amount;
	}
}
