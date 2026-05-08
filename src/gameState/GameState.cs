using Godot;
using System;

public partial class GameState : GodotObject
{
    Godot.Collections.Array<ActivePartyMember> _activePartyMembers = [];

    public GameState()
    {
        
    }

    public Godot.Collections.Array<ActivePartyMember> GetActivePartyMembers() { return _activePartyMembers; }
}
