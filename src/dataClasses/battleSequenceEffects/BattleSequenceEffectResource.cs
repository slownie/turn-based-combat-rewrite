using Godot;
using System;

[GlobalClass]
public abstract partial class BattleSequenceEffectResource : Resource
{
    [Signal] public delegate void EffectFinishedEventHandler();

    public async virtual void ExecuteSequence()
    {
        
    }
}
