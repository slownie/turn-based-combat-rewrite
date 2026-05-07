using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string name = "";
    [Export] BaseStats baseStats;
    [Export] Godot.Collections.Array skills = [];

    [Export] bool isBoss = false;

    [ExportCategory("Animation")]
    [Export] Texture icon;

    public EnemyResource() {}
}