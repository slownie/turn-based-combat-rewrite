using Godot;
using System;

[GlobalClass]
public partial class AttackEffect : ActionEffectResource
{
    [Export] int baseDamage = 10;
    [Export] int baseAccuracy = 100;
    [Export] int baseCrit = 21;
    [Export] BattleConsts.DamageCalculation damageCalculation = BattleConsts.DamageCalculation.Strength;
    [Export] BattleConsts.ElementType elementType = BattleConsts.ElementType.Phys;

    public override void ExecuteEffect(BattleActor user, BattleActor target)
    {
        int calculatedDamage = 0;
        switch(damageCalculation)
        {
            case BattleConsts.DamageCalculation.Strength:
            {
                break;
            }

            case BattleConsts.DamageCalculation.Elemental:
            {
                break;
            }

            case BattleConsts.DamageCalculation.True:
            {
                calculatedDamage = baseDamage;
                break;
            }
        }

        
        target.AddCurHP(-calculatedDamage);
    }

}
