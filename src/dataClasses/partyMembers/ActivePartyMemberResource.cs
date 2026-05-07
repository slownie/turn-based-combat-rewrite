using Godot;
using System;

public partial class ActivePartyMemberResource : Resource
{
    [Export] string partyMemberName;
    [Export] PartyMemberDataResource partyMemberDataResource;
}
