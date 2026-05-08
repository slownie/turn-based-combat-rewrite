using Godot;
using System;

[GlobalClass]
public partial class PartyMemberDataResource : Resource
{
    [Export] string partyMemberName = "";
    [Export] CharacterStats characterStats;

    [Export] int startingLevel = 1;

    public string GetPartyMemberName() { return partyMemberName; }
    public CharacterStats GetCharacterStats() { return characterStats; }
}
