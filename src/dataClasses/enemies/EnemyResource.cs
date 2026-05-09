using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] CharacterStats characterStats;

    [Export] SpriteFrames spriteFrames;

    public string GetEnemyName() { return enemyName; }
    public CharacterStats GetCharacterStats() { return characterStats; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }
}
