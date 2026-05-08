using Godot;
using System;

public partial class GameState : GodotObject
{
    Godot.Collections.Array<ActivePartyMember> _activePartyMembers = [];

    public GameState()
    {
        
    }

    public void NewGame()
    {
        PartyMemberDataResource firstPartyMemberData = GD.Load<PartyMemberDataResource>("res://data/partyMembers/Terra.tres");
        ActivePartyMember activePartyMember = new ActivePartyMember(firstPartyMemberData);
        _activePartyMembers.Add(activePartyMember);
    }

    public void SaveGameData()
    {
        
    }

    public void LoadGameData()
    {
        
    }

    public Godot.Collections.Array<ActivePartyMember> GetActivePartyMembers() { return _activePartyMembers; }
}
