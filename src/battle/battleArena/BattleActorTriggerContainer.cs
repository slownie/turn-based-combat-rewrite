using Godot;
using System;

public partial class BattleActorTriggerContainer : GodotObject
{
    Godot.Collections.Dictionary<BattleConsts.TriggerType, Godot.Collections.Array<ActivePassiveEffect>> _sideEffects = 
    new Godot.Collections.Dictionary<BattleConsts.TriggerType, Godot.Collections.Array<ActivePassiveEffect>>()
    {
        {BattleConsts.TriggerType.Never, []},
        {BattleConsts.TriggerType.OnBattleStart, []},
        {BattleConsts.TriggerType.OnBattleEnd, []},

        {BattleConsts.TriggerType.OnUserTurnStart, []},
        {BattleConsts.TriggerType.OnUserTurnEnd, []},

        {BattleConsts.TriggerType.OnUserPhysicalAttack, []},
        {BattleConsts.TriggerType.OnUserElementalAttack, []},
        
        {BattleConsts.TriggerType.OnUserTakeDamage, []},
    };

    public BattleActorTriggerContainer() {}

    public void AddSideEffect(BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        _sideEffects[triggerType].Add(activePassiveEffect);
    }

    
    public void RemoveSideEffect(BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        _sideEffects[triggerType].Remove(activePassiveEffect);
    }

    public Godot.Collections.Array<ActivePassiveEffect> GetTriggerEffects(BattleConsts.TriggerType triggerType)
    {
        return _sideEffects[triggerType];
    }
}
