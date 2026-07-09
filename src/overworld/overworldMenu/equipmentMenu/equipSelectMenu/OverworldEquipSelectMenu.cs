using Godot;

public partial class OverworldEquipSelectMenu : UIOverworldMenuBase
{
	[Export] PackedScene equipMenuEntry;

	GameState _gameState;
	InventoryController _inventoryController;

	ActivePartyMember _currentPartyMember;

	Godot.Collections.Array<EquipmentItem> _equipment = [];

	Godot.Collections.Array<EquipItemEntry> _equipmentEntries = [];
	VBoxContainer _equipmentEntryContainer;
	EquipItemEntry _currentEntry;

	UIStatsDisplay _uiStatsDisplay;

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
		_equipmentEntryContainer = GetNode<VBoxContainer>("EquipEntries");
	
		_uiStatsDisplay = GetNode<UIStatsDisplay>("UIStatsDisplay");
	}

    public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("AButton"))
		{
			if (_currentEntry.GetEnabled())
			{
				EquipEquipment();
			}
		}

		if (@event.IsActionPressed("BButton"))
		{
			EmitSignal(SignalName.ExitMenu);
		}
	}

	public void BindServices(GameState gameState, InventoryController inventoryController)
	{
		_gameState = gameState;
		_inventoryController = inventoryController;
	}

	public void Setup(int partyMemberIndex, EquipmentItemResource.EquipmentType equipmentType)
	{
		_currentPartyMember = _gameState.GetActivePartyMembers()[partyMemberIndex];

		// Get current equipment
		switch(equipmentType)
		{
			case EquipmentItemResource.EquipmentType.Weapon:
			{
				Godot.Collections.Array<EquipmentItem> _availableWeapons = _gameState.GetWeaponInventory();

				// Filter to only get weapons available to this party member
				foreach (EquipmentItem equipmentItem in _availableWeapons)
				{
					GD.Print(equipmentItem.GetEquipRestriction());
					if (equipmentItem.GetEquipRestriction() == EquipmentItemResource.EquipRestriction.Any || equipmentItem.GetEquipRestriction() == _currentPartyMember.GetEquipRestriction())
					{
						_equipment.Add(equipmentItem);
					}
				}
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

		if (_equipment.Count == 0)
		{
			// No equipment
			EquipItemEntry equipmentEntry = equipMenuEntry.Instantiate() as EquipItemEntry;
			_equipmentEntryContainer.AddChild(equipmentEntry);

			_equipmentEntries.Add(equipmentEntry);
			equipmentEntry.Setup(null);
		} else {
			// Create equipment entries
			foreach (EquipmentItem equipmentItem in _equipment)
			{
				EquipItemEntry equipmentEntry = equipMenuEntry.Instantiate() as EquipItemEntry;
				_equipmentEntryContainer.AddChild(equipmentEntry);

				_equipmentEntries.Add(equipmentEntry);
				equipmentEntry.Setup(equipmentItem);
			}
		}

		_uiStatsDisplay.Setup(_currentPartyMember.GetCharacterStats());

		index = 0;
	}

	private void EquipEquipment()
	{
		_inventoryController.EquipItem(_currentPartyMember, _currentEntry.GetEquipmentItem());
	}
}
