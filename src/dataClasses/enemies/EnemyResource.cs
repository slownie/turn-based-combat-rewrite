using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] CharacterStats characterStats;

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetEnemyName() { return enemyName; }
    public CharacterStats GetCharacterStats() { return characterStats; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }

    public Texture2D GetBattleIcon() { return battleIcon; }
}
