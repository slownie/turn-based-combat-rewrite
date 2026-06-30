using Godot;
using System;

[GlobalClass]
public partial class PartyMemberDataResource : Resource
{
    [Export] string partyMemberName = "";

    /*
        Used to identify the party member in terms of fusion skills.
    */
    [Export] int partyMemberFusionID;
    [Export] BaseStats baseStats;

    [Export] BaseAffinity baseAffinity;

    [Export] int startingLevel = 1;

    [Export] WeaponItemResource.WeaponType weaponType;

    [Export] Godot.Collections.Array<BaseSkillResource> startingSkills = [];

    // TESTING ONLY
    [Export] Godot.Collections.Array<FusionSkillResource> startingFusionSkills = [];

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetPartyMemberName() { return partyMemberName; }
    public int GetPartyMemberFusionID() { return partyMemberFusionID; }
    public BaseStats GetBaseStats() { return baseStats; }
    public WeaponItemResource.WeaponType GetWeaponType() { return weaponType; }
    public Godot.Collections.Array<BaseSkillResource> GetStartingSkills() { return startingSkills; }
    public Godot.Collections.Array<FusionSkillResource> GetStartingFusionSkills() { return startingFusionSkills; }
    public BaseAffinity GetBaseAffinity() { return baseAffinity; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }
    public Texture2D GetBattleIcon() { return battleIcon; }
}
