using Godot;
using System;

public partial class SignalGroup : RefCounted
{
    [Signal] public delegate void SignalsCompletedEventHandler();

    int _counter = 0;

    public void AddSignal()
    {
        _counter += 1;
    }

    public void StartAwait(int awaitCounter, StringName signalName)
    {
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
