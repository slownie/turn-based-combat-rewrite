using Godot;
using System;

[GlobalClass]
public partial class BaseAffinity : Resource
{
    // Very clunky but Godot doesn't support custom dictionary typing in exports yet
    [Export] BattleConsts.AffinityType _physAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _fireAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _waterAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _windAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _earthAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _steamAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _electricAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _metalAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _iceAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _lifeAffinity = BattleConsts.AffinityType.Normal;
    [Export] BattleConsts.AffinityType _gravityAffinity = BattleConsts.AffinityType.Normal;

    public BaseAffinity() {}

    public BattleConsts.AffinityType GetPhysAffinity() { return _physAffinity; }
    public BattleConsts.AffinityType GetFireAffinity() { return _fireAffinity; }
    public BattleConsts.AffinityType GetWaterAffinity() { return _waterAffinity; }
    public BattleConsts.AffinityType GetWindAffinity() { return _windAffinity; }
    public BattleConsts.AffinityType GetEarthAffinity() { return _earthAffinity; }
    public BattleConsts.AffinityType GetSteamAffinity() { return _steamAffinity; }
    public BattleConsts.AffinityType GetElectricAffinity() { return _electricAffinity; }
    public BattleConsts.AffinityType GetMetalAffinity() { return _metalAffinity; }
    public BattleConsts.AffinityType GetIceAffinity() { return _iceAffinity; }
    public BattleConsts.AffinityType GetLifeAffinity() { return _lifeAffinity; }
    public BattleConsts.AffinityType GetGravityAffinity() { return _gravityAffinity; }

}
