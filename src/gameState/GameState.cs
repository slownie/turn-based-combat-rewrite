using Godot;
using System;

public partial class GameState : GodotObject
{
    Godot.Collections.Array<ActivePartyMember> _activePartyMembers = [];
    Godot.Collections.Array<InventoryItem> _consumableItems = [];

    public GameState()
    {
        
    }

    public void NewGame(Godot.Collections.Array<PartyMemberDataResource> partyMembers, Godot.Collections.Array<UseableItemResource> startingInventory)
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
        
    }

    public void SaveGameData()
    {
        
    }

    public void LoadGameData()
    {
        
    }

    public Godot.Collections.Array<ActivePartyMember> GetActivePartyMembers() { return _activePartyMembers; }
    public Godot.Collections.Array<InventoryItem> GetInventoryItems() { return _consumableItems; }
}
