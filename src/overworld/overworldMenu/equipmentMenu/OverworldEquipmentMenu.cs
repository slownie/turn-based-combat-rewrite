using Godot;
using System;

public partial class OverworldEquipmentMenu : UIOverworldMenuBase
{
	Label _partyMemberLabel;

	EquipmentEntry _weaponEntry;
	EquipmentEntry _armorEntry;
	EquipmentEntry _accessoryEntry;

	UIStatsDisplay _statsDisplay;

	GameState _gameState;

	int _partyIndex = 0;
	int partyIndex
	{
		get { return _partyIndex; }
		set
		{
			_partyIndex = value;
			ActivePartyMember currentPartyMember = _gameState.GetActivePartyMembers()[_partyIndex];

			// Update UI elements
			string emptyText = "--------";

			_partyMemberLabel.Text = currentPartyMember.GetPartyMemberName();

			if (currentPartyMember.GetEquippedWeapon() != null)
			{
				_weaponEntry.SetText(currentPartyMember.GetEquippedWeapon().GetItemName());
			} else {
				_weaponEntry.SetText(emptyText);
			}

			if (currentPartyMember.GetEquippedArmor() != null)
			{
				_armorEntry.SetText(currentPartyMember.GetEquippedArmor().GetItemName());
			} else {
				_armorEntry.SetText(emptyText);
			}

			if (currentPartyMember.GetEquippedAccessory() != null)
			{
				_accessoryEntry.SetText(currentPartyMember.GetEquippedAccessory().GetItemName());
			} else {
				_accessoryEntry.SetText(emptyText);
			}

			_statsDisplay.Setup(currentPartyMember.GetCharacterStats());
		}
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberLabel = GetNode<Label>("PartyMemberLabel");

		_weaponEntry = GetNode<EquipmentEntry>("EquipmentList/WeaponEntry");
		_armorEntry = GetNode<EquipmentEntry>("EquipmentList/ArmorEntry");
		_accessoryEntry = GetNode<EquipmentEntry>("EquipmentList/AccessoryEntry");

		_statsDisplay = GetNode<UIStatsDisplay>("UIStatsDisplay");
	}


	public void Setup(GameState gameState)
	{
		_gameState = gameState;



		partyIndex = 0;
	}
}
