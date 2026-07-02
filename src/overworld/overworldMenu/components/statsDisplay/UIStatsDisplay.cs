using Godot;
using System;

public partial class UIStatsDisplay : Control
{
	Label _HPValue;
	Label _MPValue;
	Label _strengthValue;
	Label _elementalValue;
	Label _agilityValue;
	Label _luckValue;
	Label _defenseValue;
	Label _resistanceValue;

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
		
	}

	public void Setup(CharacterStats characterStats)
	{
		_HPValue.Text = characterStats.GetMaxHP().ToString();
		_MPValue.Text = characterStats.GetMaxMP().ToString();
		_strengthValue.Text = characterStats.GetStrength().ToString();
		_elementalValue.Text = characterStats.GetElemental().ToString();
		_agilityValue.Text = characterStats.GetAgility().ToString();
		_luckValue.Text = characterStats.GetLuck().ToString();
		_defenseValue.Text = characterStats.GetDefense().ToString();
		_resistanceValue.Text = characterStats.GetResistance().ToString();
	}
}
