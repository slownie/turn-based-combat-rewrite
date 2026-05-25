using Godot;
using System;

public partial class BattleTriggerController : Node2D
{
    [Signal] public delegate void SideEffectsRequestedEventHandler(Godot.Collections.Array<ActionEffectResource> actionEffects, BattleActor actor);


    public void SetupActor(BattleActor actor)
    {
        foreach(PassiveSkillResource passiveSkill in actor.GetPassiveSkills())
        {
            ActivePassiveSkill activePassiveSkill = new ActivePassiveSkill(passiveSkill);
            
            switch (passiveSkill.GetTriggerType())
            {
                case BattleConsts.TriggerType.OnUserTurnStart:
                {
                    break;
                }                
            }
        }
    }

    private void OnSideEffectRequested(Godot.Collections.Array<ActionEffectResource> sideEffects, BattleActor actor)
    {
        EmitSignal(SignalName.SideEffectsRequested, sideEffects, actor);
    }
}
