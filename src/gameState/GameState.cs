using Godot;
using System;

public partial class GameState : GodotObject
{
    Godot.Collections.Array<ActivePartyMember> _activePartyMembers = [];

    public GameState()
    {
        
    }

    public void NewGame(Godot.Collections.Array<PartyMemberDataResource> partyMembers)
    {
        foreach (PartyMemberDataResource partyMemberData in partyMembers)
        {
            ActivePartyMember activePartyMember = new ActivePartyMember(partyMemberData);
            _activePartyMembers.Add(activePartyMember);
        }
        
    }

    public void SaveGameData()
    {
        
    }

    public void LoadGameData()
    {
        
    }

    public Godot.Collections.Array<ActivePartyMember> GetActivePartyMembers() { return _activePartyMembers; }
}
