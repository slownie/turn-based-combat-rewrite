using Godot;
using System;

[GlobalClass]
public abstract partial class ActionEffectResource : Resource
{
    // Determines if the action should occur, primarily used for bonus effects or variable hits
    [Export] int successChance = 100;

}
