using Godot;
using System;

[GlobalClass]
public partial class StatusConditionResource : Resource
{
    [Export] string statusConditionName = "";

    [Export] Texture2D icon;


    [Export] PassiveActionResource passiveActionResource;

    [Export] BattleConsts.ElementType elementType; 
    [Export] Godot.Collections.Dictionary<BattleConsts.ElementType, StatusConditionResource> fusionStatusConditions = new Godot.Collections.Dictionary<BattleConsts.ElementType, StatusConditionResource>()
    {
        {BattleConsts.ElementType.Fire, null},
        {BattleConsts.ElementType.Water, null},
        {BattleConsts.ElementType.Wind, null},
        {BattleConsts.ElementType.Earth, null},
    };

    public PassiveActionResource GetPassiveActionResource() { return passiveActionResource; }
    public BattleConsts.TriggerType GetTriggerType() { return passiveActionResource.GetTriggerType(); }

    public BattleConsts.ElementType GetElementType() { return elementType; }
    public Godot.Collections.Dictionary<BattleConsts.ElementType, StatusConditionResource> GetFusionStatusConditions() { return fusionStatusConditions; }

    public Texture2D GetIcon() { return icon; }
}
