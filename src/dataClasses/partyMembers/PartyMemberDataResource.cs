using Godot;
using System;

[GlobalClass]
public partial class PartyMemberDataResource : Resource
{
    [Export] string partyMemberName;
    [Export] BaseStats baseStats;

    [Export] int startingLevel = 1;

    public string GetPartyMemberName() { return partyMemberName; }
    public BaseStats GetBaseStats() { return baseStats; }
}
