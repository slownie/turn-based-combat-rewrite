using Godot;
using System;

[GlobalClass]
public abstract partial class ActionEffectResource : Resource
{

    // Determines if the action should occur, primarily used for bonus effects or variable hits
    [Export] int successChance = 100;

    /*
        Runs when running the action in the overworld.
        Primarily used for healing, rejuvenating, and reviving.
        If the player somehow accesses a disabled action in the game,
        this function being blank will prevent errors/crashing.
    */
    public virtual void OverworldEffect(ActivePartyMember target) {}


    public abstract void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController);


    public int GetSuccessChance() { return successChance; }

}
