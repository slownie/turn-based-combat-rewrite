using Godot;
using System;

public partial class UIStatsDisplay : Control
{
	[Export] Color increaseColour = new Color("#00ff0d");
	[Export] Color decreaseColour = new Color("#ff0000");
	[Export] Color defaultColour = new Color("#ffffff");
	

	CharacterStats _currentCharacterStats;

	Label _HPValue;
	Label _MPValue;
	Label _strengthValue;
	Label _elementalValue;
	Label _agilityValue;
	Label _luckValue;
	Label _defenseValue;
	Label _resistanceValue;

	Label  _HPModifier;
	Label _MPModifier;
	Label _strengthModifier;
	Label _elementalModifier;
	Label _agilityModifier;
	Label _luckModifier;
	Label _defenseModifier;
	Label _resistanceModifier;

	public override void _Ready()
	{
		_HPValue = GetNode<Label>("GridContainer/HP/HPValue");
		_MPValue = GetNode<Label>("GridContainer/MP/MPValue");
		_strengthValue = GetNode<Label>("GridContainer/Strength/StrengthValue");
		_elementalValue = GetNode<Label>("GridContainer/Elemental/ElementalValue");
		_agilityValue = GetNode<Label>("GridContainer/Agility/AgilityValue");
		_luckValue = GetNode<Label>("GridContainer/Luck/LuckValue");
		_defenseValue = GetNode<Label>("GridContainer/Defense/DefenseValue");
		_resistanceValue = GetNode<Label>("GridContainer/Resistance/ResistanceValue");

		_HPModifier = GetNode<Label>("GridContainer/HP/HPModifier");
		_MPModifier = GetNode<Label>("GridContainer/MP/MPModifier");
		_strengthModifier = GetNode<Label>("GridContainer/Strength/StrengthModifier");
		_elementalModifier = GetNode<Label>("GridContainer/Elemental/ElementalModifier");
		_agilityModifier = GetNode<Label>("GridContainer/Agility/AgilityModifier");
		_luckModifier = GetNode<Label>("GridContainer/Luck/LuckModifier");
		_defenseModifier = GetNode<Label>("GridContainer/Defense/DefenseModifier");
		_resistanceModifier = GetNode<Label>("GridContainer/Resistance/ResistanceModifier");
	}

	public void Setup(CharacterStats characterStats)
	{
		// This is some crap

		if (_currentCharacterStats != null) {
			_currentCharacterStats.MaxHPChanged -= OnMaxHPChanged;
			_currentCharacterStats.MaxMPChanged -= OnMaxMPChanged;
			_currentCharacterStats.StrengthChanged -= OnStrengthChanged;
			_currentCharacterStats.ElementalChanged -= OnElementalChanged;
			_currentCharacterStats.AgilityChanged -= OnAgilityChanged;
			_currentCharacterStats.LuckChanged -= OnLuckChanged;
			_currentCharacterStats.DefenseChanged -= OnDefenseChanged;
			_currentCharacterStats.ResistanceChanged -= OnResistanceChanged;
		}

		_currentCharacterStats = characterStats;

		_HPValue.Text = _currentCharacterStats.GetMaxHP().ToString();
		_MPValue.Text = _currentCharacterStats.GetMaxMP().ToString();
		_strengthValue.Text = _currentCharacterStats.GetStrength().ToString();
		_elementalValue.Text = _currentCharacterStats.GetElemental().ToString();
		_agilityValue.Text = _currentCharacterStats.GetAgility().ToString();
		_luckValue.Text = _currentCharacterStats.GetLuck().ToString();
		_defenseValue.Text = _currentCharacterStats.GetDefense().ToString();
		_resistanceValue.Text = _currentCharacterStats.GetResistance().ToString();

		_currentCharacterStats.MaxHPChanged += OnMaxHPChanged;
		_currentCharacterStats.MaxMPChanged += OnMaxMPChanged;
		_currentCharacterStats.StrengthChanged += OnStrengthChanged;
		_currentCharacterStats.ElementalChanged += OnElementalChanged;
		_currentCharacterStats.AgilityChanged += OnAgilityChanged;
		_currentCharacterStats.LuckChanged += OnLuckChanged;
		_currentCharacterStats.DefenseChanged += OnDefenseChanged;
		_currentCharacterStats.ResistanceChanged += OnResistanceChanged;
	}

	public void DisplayStatModifiers(BaseStats baseStats)
	{
		int calculatedMaxHP = _currentCharacterStats.GetMaxHP() - (_currentCharacterStats.GetMaxHP() + baseStats.GetMaxHP());
		string displayMaxHP = calculatedMaxHP.ToString();

		if (_currentCharacterStats.GetMaxHP() < calculatedMaxHP)
		{
			_HPModifier.Modulate = increaseColour;
			displayMaxHP = "+" + displayMaxHP;
		} else if (calculatedMaxHP < _currentCharacterStats.GetMaxHP()) {
			_HPModifier.Modulate = decreaseColour;
		} else {
			_HPModifier.Modulate = defaultColour;
		}
		_HPModifier.Text = displayMaxHP;
	}

	private void OnMaxHPChanged(int newMaxHP) { _HPValue.Text = newMaxHP.ToString(); }
	private void OnMaxMPChanged(int newMaxMP) { _MPValue.Text = newMaxMP.ToString(); }
	private void OnStrengthChanged(int newStrength) { _strengthValue.Text = newStrength.ToString(); }
	private void OnElementalChanged(int newElemental) { _elementalValue.Text = newElemental.ToString(); }
	private void OnAgilityChanged(int newAgility) { _agilityValue.Text = newAgility.ToString(); }
	private void OnLuckChanged(int newLuck) { _luckValue.Text = newLuck.ToString(); }
	private void OnDefenseChanged(int newDefense) { _defenseValue.Text = newDefense.ToString(); }
	private void OnResistanceChanged(int newResistance) { _resistanceValue.Text = newResistance.ToString(); }

}
