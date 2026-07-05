using Godot;
using System;

public partial class GameState : GodotObject
{
    Godot.Collections.Array<ActivePartyMember> _activePartyMembers = [];

    // Does it make sense to have these seperate arrays? We'll find out soon enough
    Godot.Collections.Array<InventoryItem> _consumableItems = [];
    Godot.Collections.Array<EquipmentItem> _weaponInventory = [];
    Godot.Collections.Array<EquipmentItem> _armorInventory = [];
    Godot.Collections.Array<EquipmentItem> _accessoryInventory = [];
    Godot.Collections.Array<InventoryItem> _keyItems = [];

    int _goldAmount;

    public GameState()
    {
        
    }

    public void NewGame(Godot.Collections.Array<PartyMemberDataResource> partyMembers, 
        Godot.Collections.Array<BaseItemResource> startingInventory,
        Godot.Collections.Array<EquipmentItemResource> equipmentInventory
    )
    {
        foreach (PartyMemberDataResource partyMemberData in partyMembers)
        {
            ActivePartyMember activePartyMember = new ActivePartyMember(partyMemberData);
            _activePartyMembers.Add(activePartyMember);
        }

        foreach (UseableItemResource itemResource in startingInventory)
        {
            InventoryItem inventoryItem = new InventoryItem(itemResource, 3);
            _consumableItems.Add(inventoryItem);
        }

        foreach (EquipmentItemResource itemResource in equipmentInventory)
        {
            EquipmentItem equipmentItem = new EquipmentItem(itemResource);
            switch (itemResource.GetEquipmentType())
            {
                case EquipmentItemResource.EquipmentType.Weapon:
                {
                    _weaponInventory.Add(equipmentItem);
                    break;
                }
                case EquipmentItemResource.EquipmentType.Armor:
                {
                    _armorInventory.Add(equipmentItem);
                    break;
                }
                case EquipmentItemResource.EquipmentType.Accessory:
                {
                    _accessoryInventory.Add(equipmentItem);
                    break;
                }
            }
        }
        
        _goldAmount = 100;
    }

    public void SaveGameData()
    {
        
    }

    public void LoadGameData()
    {
        
    }

    public Godot.Collections.Array<ActivePartyMember> GetActivePartyMembers() { return _activePartyMembers; }
    public Godot.Collections.Array<InventoryItem> GetInventoryItems() { return _consumableItems; }

    public Godot.Collections.Array<EquipmentItem> GetWeaponInventory() { return _weaponInventory; }
    public Godot.Collections.Array<EquipmentItem> GetArmorInventory() { return _armorInventory; }
    public Godot.Collections.Array<EquipmentItem> GetAccessoryInventory() { return _accessoryInventory; }
    
    public Godot.Collections.Array<InventoryItem> GetKeyItems() { return _keyItems; }

    public int GetGoldAmount() { return _goldAmount; }
}
