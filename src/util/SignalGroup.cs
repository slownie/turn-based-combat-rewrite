using Godot;
using System;

public partial class SignalGroup : RefCounted
{
    [Signal] public delegate void SignalsCompletedEventHandler();

    int _counter = 0;

    public SignalGroup()
    {
        
    }

    public void StartAwait(Godot.Collections.Array<HitEffect> hitEffects)
    {
        _counter = hitEffects.Count;
        foreach (HitEffect hitEffect in hitEffects)
        {
            hitEffect.HitEffectFinished += OnSignalComplete;
        }
    }

    private void OnSignalComplete()
    {
        _counter -= 1;
        if (_counter == 0)
        {
            EmitSignal(SignalName.SignalsCompleted);
        }
    }
}
