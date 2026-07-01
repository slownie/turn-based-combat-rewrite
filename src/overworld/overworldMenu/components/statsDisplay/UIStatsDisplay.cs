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

	public void Setup()
	{
		
	}

}
