using Godot;
using System;


/*
    The purpose of the BattleTriggerController is to keep track of the various 'passive' effects.

*/
public partial class BattleTriggerController : Node2D
{
    [Signal] public delegate void SideEffectsRequestedEventHandler(ActivePassiveEffect activePassiveEffect, EffectDirective effectDirective, BattleActor battleActor);

    Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer> _triggerContainers = new Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer>();

    public enum EffectDirective
    {
        Startup,
        Trigger,
        Cleanup
    }

    public void CreateActorContainer(BattleActor battleActor)
    {
        BattleActorTriggerContainer battleActorTriggerContainer = new BattleActorTriggerContainer();
        if (!_triggerContainers.ContainsKey(battleActor))
        {
            _triggerContainers.Add(battleActor, battleActorTriggerContainer);
        }

        battleActor.AddSideEffect += AddSideEffect;
        battleActor.RemoveSideEffect += RemoveSideEffect;
    }

    public void RunActorSideEffects(BattleActor wantedActor, BattleConsts.TriggerType triggerType)
    {
        BattleActorTriggerContainer battleActorTriggerContainer;
        battleActorTriggerContainer = _triggerContainers[wantedActor];
        if (battleActorTriggerContainer != null)
        {
            // Get Side Effects
            foreach (ActivePassiveEffect activePassiveEffect in battleActorTriggerContainer.GetTriggerEffects(triggerType))
            {
                // Only run effects that have not been ran
                if (!activePassiveEffect.GetHasBeenRan())
                {
                    if (activePassiveEffect.GetRunOnce())
                    {
                       activePassiveEffect.SetHasBeenRan(true); 
                    }
                    EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect, (int)EffectDirective.Trigger, wantedActor);
                }
            }
        }
    }

    private void AddSideEffect(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        _triggerContainers[user].AddSideEffect(triggerType, activePassiveEffect);
        if (activePassiveEffect.GetStartupEffects().Count != 0)
        {
            EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect, (int)EffectDirective.Startup, user);
        }
    }

    private void RemoveSideEffect(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        if (activePassiveEffect.GetCleanupEffects().Count != 0)
        {
            EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect, (int)EffectDirective.Cleanup, user);
        }
        _triggerContainers[user].RemoveSideEffect(triggerType, activePassiveEffect);
    }
}