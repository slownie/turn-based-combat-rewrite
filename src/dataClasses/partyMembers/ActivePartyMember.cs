using Godot;
using System;

public partial class ActivePartyMember : GodotObject
{
    // UI
    string _name = "PartyMember - Name Placeholder";

    // Stats
    int currentLevel = 1;
    int currentEXP = 0;

    // Equipment

    // Skills
    Godot.Collections.Array _learnedSkills = [];
    Godot.Collections.Array<int> _equippedSkills = []; 

    public ActivePartyMember() : this(null) {}
    public ActivePartyMember(PartyMemberDataResource partyMemberDataResource)
    {
        if (partyMemberDataResource != null)
        {
            _name = partyMemberDataResource.GetPartyMemberName();
        } else {
            GD.Print("ActivePartyMember - partyMemberDataResource is null");
        }
    }


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
