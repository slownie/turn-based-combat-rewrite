using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] CharacterStats characterStats;

    public string GetEnemyName() { return enemyName; }
    public CharacterStats GetCharacterStats() { return characterStats; }
}
