using Godot;
using System;

[GlobalClass]
public partial class CharacterAffinity : GodotObject
{
    Godot.Collections.Dictionary<BattleConsts.ElementType, BattleConsts.AffinityType> _affinity = new Godot.Collections.Dictionary<BattleConsts.ElementType, BattleConsts.AffinityType>()
	{
		{BattleConsts.ElementType.Phys, BattleConsts.AffinityType.Normal},	
		{BattleConsts.ElementType.Fire, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Water, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Wind, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Earth, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Steam, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Electric, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Metal, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Ice, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Life, BattleConsts.AffinityType.Normal},
		{BattleConsts.ElementType.Gravity, BattleConsts.AffinityType.Normal},
	};

    public CharacterAffinity() : this(null) {}
    public CharacterAffinity(BaseAffinity baseAffinity)
    {
        // BaseAffinity isn't a dict so has to be done like this unfortunately
        SetAffinity(BattleConsts.ElementType.Phys, baseAffinity.GetPhysAffinity());
        SetAffinity(BattleConsts.ElementType.Fire, baseAffinity.GetFireAffinity());
        SetAffinity(BattleConsts.ElementType.Water, baseAffinity.GetWaterAffinity());
        SetAffinity(BattleConsts.ElementType.Wind, baseAffinity.GetWindAffinity());
        SetAffinity(BattleConsts.ElementType.Earth, baseAffinity.GetEarthAffinity());
        SetAffinity(BattleConsts.ElementType.Steam, baseAffinity.GetSteamAffinity());
        SetAffinity(BattleConsts.ElementType.Electric, baseAffinity.GetElectricAffinity());
        SetAffinity(BattleConsts.ElementType.Metal, baseAffinity.GetMetalAffinity());
        SetAffinity(BattleConsts.ElementType.Ice, baseAffinity.GetIceAffinity());
        SetAffinity(BattleConsts.ElementType.Life, baseAffinity.GetLifeAffinity());
        SetAffinity(BattleConsts.ElementType.Gravity, baseAffinity.GetGravityAffinity());
    }

    public BattleConsts.AffinityType GetAffinity(BattleConsts.ElementType elementKey) { return _affinity[elementKey]; }
    public void SetAffinity(BattleConsts.ElementType elementKey, BattleConsts.AffinityType affinityType) { _affinity[elementKey] = affinityType; }
}
