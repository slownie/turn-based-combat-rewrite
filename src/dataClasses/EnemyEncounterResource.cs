using Godot;
using System;

[GlobalClass]
public partial class EnemyEncounterResource : Resource
{
    [Export] Godot.Collections.Array<EnemyResource> enemies = [];
    [Export] int goldGain = 0;
    [Export] int expGain = 0;
    [Export] Godot.Collections.Array itemDrops = [];
    [Export] AudioStream musicTrack;
}
