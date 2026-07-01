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
    int _partyMemberFusionID;


    // Equipment
    EquipmentItem _equippedWeapon = null;
    EquipmentItem _equippedArmor = null;
    EquipmentItem _equippedAccessory = null;

    // Skills
    Godot.Collections.Array<BaseSkillResource> _learnedSkills = [];
    Godot.Collections.Array<BaseSkillResource> _equippedSkills = []; 

    Godot.Collections.Array<FusionSkillResource> _learnedFusionSkills = [];
    Godot.Collections.Array<FusionSkillResource> _equippedFusionSkills = [];

    CharacterAffinity _characterAffinity;

    // Battle Animation
    PackedScene _battleAnimScene;
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
            _partyMemberFusionID = partyMemberDataResource.GetPartyMemberFusionID();

            // Skill Setup
            _learnedSkills = partyMemberDataResource.GetStartingSkills();
            _equippedSkills = _learnedSkills;

            // TESTING ONLY
            _learnedFusionSkills = partyMemberDataResource.GetStartingFusionSkills();
            _equippedFusionSkills = _learnedFusionSkills;


            _characterAffinity = new CharacterAffinity(partyMemberDataResource.GetBaseAffinity());

            _battleAnimScene = partyMemberDataResource.GetBattleAnimScene();
            _battleIcon = partyMemberDataResource.GetBattleIcon();
        } else {
            GD.Print("ActivePartyMember - partyMemberDataResource not found, disregard if this is at the start of the program");
        }
    }

    public string GetPartyMemberName() { return _name; }
    public int GetPartyMemberFusionID() { return _partyMemberFusionID; }
    public CharacterStats GetCharacterStats() { return _characterStats; }
    public Godot.Collections.Array<BaseSkillResource> GetEquippedSkills() { return _equippedSkills; }
    public Godot.Collections.Array<FusionSkillResource> GetEquippedFusionSkills() { return _equippedFusionSkills; }
    public CharacterAffinity GetCharacterAffinity() { return _characterAffinity; }

    public PackedScene GetBattleAnimScene() { return _battleAnimScene; }
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

    /*
        Equipment
    */
    public void EquipWeapon(EquipmentItem weaponItem)
    {
        // Unequip the current weapon if it exists
        if (_equippedWeapon != null)
        {
            RemoveStats(_equippedWeapon.GetEquipmentBaseStats());
            _equippedWeapon.RemoveUser();
        }

        _equippedWeapon = weaponItem;
        ApplyStats(_equippedWeapon.GetEquipmentBaseStats());
        _equippedWeapon.SetUser(this);
    }

    private void ApplyStats(BaseStats baseStats)
    {
        // I'm not even sure if this is computitionally more efficient but w/e
        if (baseStats.GetMaxHP() != 0) _characterStats.ApplyMaxHP(baseStats.GetMaxHP());
        if (baseStats.GetMaxMP() != 0) _characterStats.ApplyMaxMP(baseStats.GetMaxMP());
        if (baseStats.GetStrength() != 0) _characterStats.ApplyStrength(baseStats.GetStrength());
        if (baseStats.GetElemental() != 0) _characterStats.ApplyElemental(baseStats.GetElemental());
        if (baseStats.GetAgility() != 0) _characterStats.ApplyAgility(baseStats.GetAgility());
        if (baseStats.GetLuck() != 0) _characterStats.ApplyLuck(baseStats.GetLuck());
        if (baseStats.GetDefense() != 0) _characterStats.ApplyDefense(baseStats.GetDefense());
        if (baseStats.GetResistance() != 0) _characterStats.ApplyResistance(baseStats.GetResistance());
    }

    private void RemoveStats(BaseStats baseStats)
    {
        if (baseStats.GetMaxHP() != 0) _characterStats.ApplyMaxHP(baseStats.GetMaxHP());
        if (baseStats.GetMaxMP() != 0) _characterStats.ApplyMaxMP(baseStats.GetMaxMP());
        if (baseStats.GetStrength() != 0) _characterStats.ApplyStrength(baseStats.GetStrength());
        if (baseStats.GetElemental() != 0) _characterStats.ApplyElemental(baseStats.GetElemental());
        if (baseStats.GetAgility() != 0) _characterStats.ApplyAgility(baseStats.GetAgility());
        if (baseStats.GetLuck() != 0) _characterStats.ApplyLuck(baseStats.GetLuck());
        if (baseStats.GetDefense() != 0) _characterStats.ApplyDefense(baseStats.GetDefense());
        if (baseStats.GetResistance() != 0) _characterStats.ApplyResistance(baseStats.GetResistance());
    }
}