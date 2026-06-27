using Godot;
using System;


/*
    The purpose of the BattleTriggerController is to keep track of the various 'passive' effects.

*/
public partial class BattleTriggerController : Node2D
{
    [Signal] public delegate void SideEffectsRequestedEventHandler(ActivePassiveEffect activePassiveEffect, BattleActor battleActor);

    Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer> _triggerContainers = new Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer>();


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
                    EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect, wantedActor);
                }
            }
        }
    }

    private void AddSideEffect(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        activePassiveEffect.RequestDeletion += OnRequestDeletion;

        _triggerContainers[user].AddSideEffect(triggerType, activePassiveEffect);
        GD.Print(triggerType+" - "+activePassiveEffect);
        if (activePassiveEffect.GetStartupEffects().Count != 0)
        {
            EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect.GetStartupEffects(), user);
        }
    }

    private void RemoveSideEffect(BattleActor user, BattleConsts.TriggerType triggerType, ActivePassiveEffect activePassiveEffect)
    {
        if (activePassiveEffect.GetCleanupEffects().Count != 0)
        {
            EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect.GetCleanupEffects(), user);
        }
        _triggerContainers[user].RemoveSideEffect(triggerType, activePassiveEffect);
    }

    private void OnRequestDeletion(ActivePassiveEffect activePassiveEffect, BattleActor user)
    {
        GD.Print("Delete Self");
        RemoveSideEffect(user, activePassiveEffect.GetTriggerType(), activePassiveEffect);
    }
}