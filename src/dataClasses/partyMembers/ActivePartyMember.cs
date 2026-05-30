using Godot;
using System;

/*
    This class is primarily used in the Overworld as Battles create their own actors which use
    different variables of ActivePartyMember. 
*/
public partial class ActivePartyMember : GodotObject
{
    // UI
    string _name = "PartyMember - Name Placeholder";

    // Stats
    CharacterStats _characterStats;
    int currentLevel = 1;
    int currentEXP = 0;

    // Equipment

    // Skills
    Godot.Collections.Array<BaseSkillResource> _learnedSkills = [];
    Godot.Collections.Array<BaseSkillResource> _equippedSkills = []; 

    CharacterAffinity _characterAffinity;

    // Battle Animation
    SpriteFrames _spriteFrames;
    Texture2D _battleIcon;

    public ActivePartyMember() : this(null) {}
    public ActivePartyMember(PartyMemberDataResource partyMemberDataResource)
    {
        /* 
            Necessary since Godot seems to run empty constructors before the game loads
            Good to have a safety check anyway.  
        */
        if (partyMemberDataResource != null)
        {
            _name = partyMemberDataResource.GetPartyMemberName();
            _characterStats = new CharacterStats(partyMemberDataResource.GetBaseStats());

            // Skill Setup
            _learnedSkills = partyMemberDataResource.GetStartingSkills();
            _equippedSkills = _learnedSkills;

            _characterAffinity = new CharacterAffinity(partyMemberDataResource.GetBaseAffinity());

            _spriteFrames = partyMemberDataResource.GetSpriteFrames();
            _battleIcon = partyMemberDataResource.GetBattleIcon();
        } else {
            GD.Print("ActivePartyMember - partyMemberDataResource not found, disregard if this is at the start of the program");
        }
    }

    public string GetPartyMemberName() { return _name; }
    public CharacterStats GetCharacterStats() { return _characterStats; }
    public Godot.Collections.Array<BaseSkillResource> GetEquippedSkills() { return _equippedSkills; }
    public CharacterAffinity GetCharacterAffinity() { return _characterAffinity; }

    public SpriteFrames GetSpriteFrames() { return _spriteFrames; }
    public Texture2D GetBattleIcon() { return _battleIcon; }

    /*
        Add will add the specified amount to HP/MP
        Set will set HP/MP to the specified amount 
    */
    public void AddPartyMemberHP(int amount)
    {
        
    }

    public void SetPartyMemberHP(int amount)
	{
		
	}

	public void AddPartyMemberMP(int amount)
	{
		
	}

    public void SetPartyMemberMP(int amount)
    {
        
    }
}
