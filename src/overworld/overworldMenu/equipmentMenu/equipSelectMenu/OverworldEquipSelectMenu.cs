using Godot;
using System;

public partial class OverworldEquipSelectMenu : UIOverworldMenuBase
{
	[Export] PackedScene equipMenuEntry;

	GameState _gameState;
	ActivePartyMember _currentPartyMember;

	Godot.Collections.Array<EquipmentItem> _equipment = [];

	Godot.Collections.Array<EquipmentEntry> _equipmentEntries = [];
	HBoxContainer _equipmentEntryContainer;
	EquipmentEntry _currentEntry;

	int _index = 0;
	int index
	{
		get { return _index; }
		set
		{
			if (_currentEntry != null)
			{
				_currentEntry.Highlighted = false;
			}

			_index = value;
			if (_equipmentEntries.Count <= _index) _index = 0;
			if (_index < 0) _index = _equipmentEntries.Count - 1;

			_currentEntry = _equipmentEntries[_index];
			_currentEntry.Highlighted = true;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_equipmentEntryContainer = GetNode<HBoxContainer>("EquipEntries");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.ExitMenu);
		}
	}

	public void Setup(GameState gameState, int partyMemberIndex, EquipmentItemResource.EquipmentType equipmentType)
	{
		_gameState = gameState;
		_currentPartyMember = _gameState.GetActivePartyMembers()[partyMemberIndex];

		// Get current equipment
		switch(equipmentType)
		{
			case EquipmentItemResource.EquipmentType.Weapon:
			{
				_equipment = _gameState.GetWeaponInventory();
				// Filter to only get weapons available to this party member
				break;
			}
			case EquipmentItemResource.EquipmentType.Armor:
			{
				_equipment = _gameState.GetArmorInventory();
				break;
			}
			case EquipmentItemResource.EquipmentType.Accessory:
			{
				_equipment = _gameState.GetAccessoryInventory();
				break;
			}
		}

		// Create equipment entries

	}

	private void EquipEquipment()
	{
		
	}
}
