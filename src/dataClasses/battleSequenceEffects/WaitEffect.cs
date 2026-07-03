using Godot;
using System;

[GlobalClass]
public partial class WaitEffect : BattleSequenceEffectResource
{
    [Export] double seconds;

    public double GetSeconds() { return seconds; }
}
