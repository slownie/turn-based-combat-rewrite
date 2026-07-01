using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] string enemyName = "";
    [Export] BaseStats baseStats;
    [Export] BaseAffinity baseAffinity;

    [Export] Godot.Collections.Array<BaseSkillResource> skills = [];

    [Export(PropertyHint.Range, "0,4")] int followerActorAmount = 0;

    [Export] PackedScene battleAnimScene;
    [Export] Texture2D battleIcon;

    public string GetEnemyName() { return enemyName; }
    public BaseStats GetBaseStats() { return baseStats; }
    public Godot.Collections.Array<BaseSkillResource> GetSkills() { return skills; }
    public BaseAffinity GetBaseAffinity() { return baseAffinity; }

    public int GetFollowerActorAmount() { return followerActorAmount; }

    public PackedScene GetBattleAnimScene() { return battleAnimScene; }

    public Texture2D GetBattleIcon() { return battleIcon; }
}
