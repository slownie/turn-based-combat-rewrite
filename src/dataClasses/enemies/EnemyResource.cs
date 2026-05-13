using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] BaseStats baseStats;
    [Export] Godot.Collections.Array<BaseSkillResource> skills = [];

    [Export] SpriteFrames spriteFrames;
    [Export] Texture2D battleIcon;

    public string GetEnemyName() { return enemyName; }
    public BaseStats GetBaseStats() { return baseStats; }
    public Godot.Collections.Array<BaseSkillResource> GetSkills() { return skills; }


    public SpriteFrames GetSpriteFrames() { return spriteFrames; }

    public Texture2D GetBattleIcon() { return battleIcon; }
}
