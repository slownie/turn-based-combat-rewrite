using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] BaseStats baseStats;

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetEnemyName() { return enemyName; }
    public BaseStats GetBaseStats() { return baseStats; }

    public SpriteFrames GetSpriteFrames() { return spriteFrames; }

    public Texture2D GetBattleIcon() { return battleIcon; }
}
