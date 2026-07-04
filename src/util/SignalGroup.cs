using Godot;
using System;

public partial class SignalGroup : RefCounted
{
    [Signal] public delegate void SignalsCompletedEventHandler();

    int _counter = 0;

    public SignalGroup()
    {
        
    }

    public void StartAwait(Godot.Collections.Array signalGroup, string signalName)
    {
        _counter = signalGroup.Count;
        foreach (Node node in signalGroup)
        {
            node.Connect(signalName, Callable.From(OnSignalComplete), (uint)GodotObject.ConnectFlags.OneShot);
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
