using Godot;
using System;

[GlobalClass]
public partial class PartyMemberDataResource : Resource
{
    [Export] string partyMemberName = "";
    [Export] CharacterStats characterStats;


    [Export] int startingLevel = 1;

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetPartyMemberName() { return partyMemberName; }
    public CharacterStats GetCharacterStats() { return characterStats; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }
    public Texture2D GetBattleIcon() { return battleIcon; }
}
