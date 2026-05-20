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

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        if (baseAccuracy > GD.Randi() % 99)
        {
            // Hit
            int calculatedDamage = 0;
            bool didCrit = false;

            switch(damageCalculation)
            {
                case BattleConsts.DamageCalculation.Strength:
                {
                    calculatedDamage = baseDamage + user.GetStrength();
                    break;
                }

                case BattleConsts.DamageCalculation.Elemental:
                {
                    calculatedDamage = baseDamage + user.GetElemental();
                    break;
                }

                case BattleConsts.DamageCalculation.True:
                {
                    calculatedDamage = baseDamage;
                    break;
                }
            }

            actorController.TakeDamage(target, -calculatedDamage, didCrit);
        } else {
            // Miss
            actorController.ActionMissed(target);
        }
    }

}
