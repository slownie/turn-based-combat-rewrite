using Godot;
using System;


/*
    The purpose of the BattleTriggerController is to keep track of the various 'passive' effects.

*/
public partial class BattleTriggerController : Node2D
{
    [Signal] public delegate void SideEffectsRequestedEventHandler(Godot.Collections.Array<ActionEffectResource> actions, BattleActor battleActor);

    Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer> _triggerContainers = new Godot.Collections.Dictionary<BattleActor, BattleActorTriggerContainer>();


    public void CreateActorContainer(BattleActor battleActor)
    {
        BattleActorTriggerContainer battleActorTriggerContainer = new BattleActorTriggerContainer(battleActor);
        if (!_triggerContainers.ContainsKey(battleActor))
        {
            _triggerContainers.Add(battleActor, battleActorTriggerContainer);
        }
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
                EmitSignal(SignalName.SideEffectsRequested, activePassiveEffect.GetTriggerEffects(), wantedActor);
            }
        }
    }
}