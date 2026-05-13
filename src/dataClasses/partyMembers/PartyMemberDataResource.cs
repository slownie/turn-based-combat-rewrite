using Godot;
using System;

[GlobalClass]
public partial class PartyMemberDataResource : Resource
{
    [Export] string partyMemberName = "";
    [Export] BaseStats baseStats;


    [Export] int startingLevel = 1;

    [Export] Godot.Collections.Array<BaseSkillResource> startingSkills = [];

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetPartyMemberName() { return partyMemberName; }
    public BaseStats GetBaseStats() { return baseStats; }
    public Godot.Collections.Array<BaseSkillResource> GetStartingSkills() { return startingSkills; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }
    public Texture2D GetBattleIcon() { return battleIcon; }
}
