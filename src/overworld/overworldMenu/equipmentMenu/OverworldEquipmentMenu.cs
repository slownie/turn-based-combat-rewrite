using Godot;
using System;

public partial class OverworldEquipmentMenu : UIOverworldMenuBase
{
	[Signal] public delegate void CreateEquipSelectMenuEventHandler(PackedScene equipSelectMenuScene, int partyMemberIndex, EquipmentItemResource.EquipmentType equipmentType);

	[Export] PackedScene equipSelectMenuScene;

	Label _partyMemberLabel;

	VBoxContainer _equipmentList;
	EquipmentEntry _weaponEntry;
	EquipmentEntry _armorEntry;
	EquipmentEntry _accessoryEntry;

	UIStatsDisplay _statsDisplay;

	GameState _gameState;

	int _equipmentListCount = 0;

	EquipmentEntry _currentEntry;
	int _equipmentIndex = 0;
	int equipmentIndex
	{
		get { return _equipmentIndex; }
		set
		{
			// Unhighlight the current entry
			if (_currentEntry != null)
			{
				_currentEntry.Highlighted = false;
			}

			_equipmentIndex = value;

			// Clamp
			if (_equipmentListCount <= _equipmentIndex) _equipmentIndex = 0;
			if (_equipmentIndex < 0) _equipmentIndex = _equipmentListCount - 1;

			// Update entry, this sucks but it's what you get when you try merging two implementations together
			switch(_equipmentIndex)
			{
				case 0:
				{
					_currentEntry = _weaponEntry;
					break;
				}

				case 1:
				{
					_currentEntry = _armorEntry;
					break;
				}

				case 2:
				{
					_currentEntry = _accessoryEntry;
					break;
				}
			}
			_currentEntry.Highlighted = true;
		}
	}

	int _partyIndex = 0;
	int partyIndex
	{
		get { return _partyIndex; }
		set
		{
			// Disconnect signals
			if (_currentPartyMember != null)
			{
				_currentPartyMember.EquippedWeaponChanged -= OnEquippedWeaponChanged;
				_currentPartyMember.EquippedArmorChanged -= OnEquippedArmorChanged;
				_currentPartyMember.EquippedAccessoryChanged -= OnEquippedAccessoryChanged;
			}

			_partyIndex = value;

			// Clamp
			if (_gameState.GetActivePartyMembers().Count <= _partyIndex) _partyIndex = 0;
			if (_partyIndex < 0) _partyIndex = _gameState.GetActivePartyMembers().Count - 1;

			ActivePartyMember currentPartyMember = _gameState.GetActivePartyMembers()[_partyIndex];
			_currentPartyMember = currentPartyMember;

			_currentPartyMember.EquippedWeaponChanged += OnEquippedWeaponChanged;
			_currentPartyMember.EquippedArmorChanged += OnEquippedArmorChanged;
			_currentPartyMember.EquippedAccessoryChanged += OnEquippedAccessoryChanged;

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
	ActivePartyMember _currentPartyMember; // has to be set to get access to its signals

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_partyMemberLabel = GetNode<Label>("PartyMemberLabel");

		_equipmentList = GetNode<VBoxContainer>("EquipmentList");
		_equipmentListCount = _equipmentList.GetChildren().Count;

		_weaponEntry = GetNode<EquipmentEntry>("EquipmentList/WeaponEntry");
		_armorEntry = GetNode<EquipmentEntry>("EquipmentList/ArmorEntry");
		_accessoryEntry = GetNode<EquipmentEntry>("EquipmentList/AccessoryEntry");


		_statsDisplay = GetNode<UIStatsDisplay>("UIStatsDisplay");
	}

    public override void _Input(InputEvent @event)
	{
		// Party Index
		if (@event.IsActionPressed("LButton")) { partyIndex -= 1; }
		if (@event.IsActionPressed("RButton")) { partyIndex += 1; }

		// Equipment Index
		if (@event.IsActionPressed("MoveUp")) { equipmentIndex -= 1; }
		if (@event.IsActionPressed("MoveDown")) { equipmentIndex += 1; }

		// Create Menu
		if (@event.IsActionPressed("AButton"))
		{
			EmitSignal(SignalName.CreateEquipSelectMenu, equipSelectMenuScene, partyIndex, (int)_currentEntry.GetEquipmentType());
		}

		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.ExitMenu);
		}
	}

	public void Setup(GameState gameState)
	{
		_gameState = gameState;



		partyIndex = 0;
		equipmentIndex = 0;
	}

	private void OnEquippedWeaponChanged(EquipmentItem newWeapon)
	{
		GD.Print("Recevied");
		_weaponEntry.Setup(newWeapon);
	}

	private void OnEquippedArmorChanged(EquipmentItem newArmor)
	{
		_armorEntry.Setup(newArmor);
	}

	private void OnEquippedAccessoryChanged(EquipmentItem newAccessory)
	{
		_accessoryEntry.Setup(newAccessory);
	}
}
