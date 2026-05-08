using Godot;
using System;

[GlobalClass]
public partial class EnemyEncounterResource : Resource
{
    [Export] Godot.Collections.Array<EnemyResource> enemies = [];
    [Export] int experienceDrop = 0;
    [Export] int goldDrop = 0;
    [Export] Godot.Collections.Array itemDrops = [];
    [Export] AudioStream battleMusic;

    public Godot.Collections.Array<EnemyResource> GetEnemies() { return enemies; }
    public int GetExperienceDrop() { return experienceDrop; }
    public int GetGoldDrop() { return goldDrop; }
    public Godot.Collections.Array GetItemDrops() { return itemDrops; }
    public AudioStream GetBattleMusic() { return battleMusic; }
}