using Godot;
using System;

[GlobalClass]
public partial class AttackEffect : ActionEffectResource
{
    const int _chargeMultiplier = 3;
    const int _focusMultiplier = 3;

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
                    calculatedDamage = Mathf.RoundToInt((baseDamage + user.GetStrength()) * user.GetStrengthModifier());
                    if (user.IsChargeEnabled()) calculatedDamage *= _chargeMultiplier;
                    break;
                }

                case BattleConsts.DamageCalculation.Elemental:
                {
                    calculatedDamage = Mathf.RoundToInt((baseDamage + user.GetElemental()) * user.GetElementalModifier());
                    if (user.IsFocusEnabled()) calculatedDamage *= _focusMultiplier;
                    break;
                }

                case BattleConsts.DamageCalculation.True:
                {
                    calculatedDamage = baseDamage;
                    break;
                }
            }

            // Affinity Calculation
            if (user.IgnoreAffinity) GD.Print("It pierced through the resistance!");

            actorController.TakeDamage(target, -calculatedDamage, didCrit);
        } else {
            // Miss
            actorController.ActionMissed(target);
        }
    }

}
