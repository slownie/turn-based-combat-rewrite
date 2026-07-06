using Godot;
using System;

[GlobalClass]
public partial class CharacterStats : GodotObject
{
	[Signal] public delegate void HPChangedEventHandler(int newHP);
	[Signal] public delegate void MPChangedEventHandler(int newMP);

	[Signal] public delegate void HPReviveEventHandler();
	[Signal] public delegate void HPDepletedEventHandler();

	[Signal] public delegate void MaxHPChangedEventHandler(int newMaxHP);
	[Signal] public delegate void MaxMPChangedEventHandler(int newMaxMP);
	[Signal] public delegate void StrengthChangedEventHandler(int newStrength);
	[Signal] public delegate void ElementalChangedEventHandler(int newElemental);
	[Signal] public delegate void AgilityChangedEventHandler(int newAgility);
	[Signal] public delegate void LuckChangedEventHandler(int newLuck);
	[Signal] public delegate void DefenseChangedEventHandler(int newDefense);
	[Signal] public delegate void ResistanceChangedEventHandler(int newResistance);

	int _maxHP = 0;
	int maxHP
	{
		get { return _maxHP; }
		set
		{
			_maxHP = value;
			EmitSignal(SignalName.MaxHPChanged, _maxHP);
		}
	}

	int _maxMP = 0;
	int maxMP
	{
		get { return _maxMP; }
		set
		{
			_maxMP = value;
			EmitSignal(SignalName.MaxMPChanged, _maxMP);
		}
	}

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
			EmitSignal(SignalName.HPChanged, curHP);

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
			EmitSignal(SignalName.MPChanged, curMP);

			if (_curHP < 0) { _curMP = 0; }
			if (_maxMP < _curMP) { _curMP = _maxMP; }
		}
	}

	int _strength = 0;
	int strength
	{
		get { return _strength; }
		set
		{
			_strength = value;
			EmitSignal(SignalName.StrengthChanged, _strength);
		}
	}

	int _elemental = 0;
	int elemental
	{
		get { return _elemental; }
		set
		{
			_elemental = value;
			EmitSignal(SignalName.ElementalChanged, _elemental);
		}
	}

	int _agility = 0;
	int agility
	{
		get { return _agility; }
		set
		{
			_agility = value;
			EmitSignal(SignalName.AgilityChanged, _agility);
		}
	}

	int _luck = 0;
	int luck
	{
		get { return _luck; }
		set
		{
			_luck = value;
			EmitSignal(SignalName.LuckChanged, _luck);
		}
	}
	
	int _defense = 0;
    int defense
	{
		get { return _defense; }
		set
		{
			_defense = value;
			EmitSignal(SignalName.DefenseChanged, _defense);
		}
	}

	int _resistance = 0;
	int resistance
	{
		get { return _resistance; }
		set
		{
			_resistance = value;
			EmitSignal(SignalName.ResistanceChanged, _resistance);
		}
	}

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
		curMP += amount;
	}

	public void SetCurMP(int amount)
	{
		curMP = amount;
	}

	public void ApplyMaxHP(int newMaxHP) { maxHP += newMaxHP; }
	public void ApplyMaxMP(int newMaxMP) { maxMP += newMaxMP; }
	public void ApplyStrength(int newStrength) { strength += newStrength; }
	public void ApplyElemental(int newElemental) { elemental += newElemental; }
	public void ApplyAgility(int newAgility) { agility += newAgility; }
	public void ApplyLuck(int newLuck) { luck += newLuck; }
	public void ApplyDefense(int newDefense) { defense += newDefense; }
	public void ApplyResistance(int newResistance) { resistance += newResistance; }

}
