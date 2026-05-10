using Godot;
using System;

[GlobalClass]
public partial class AttackEffect : ActionEffectResource
{
    [Export] int baseDamage = 10;
    [Export] int baseAccuracy = 100;
    [Export] int baseCrit = 21;
    [Export] BattleConsts.DamageType damageType = BattleConsts.DamageType.Physical;
    [Export] BattleConsts.ElementType elementType = BattleConsts.ElementType.Phys;
}
