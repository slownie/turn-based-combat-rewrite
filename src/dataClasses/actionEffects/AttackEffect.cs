using Godot;
using System;

[GlobalClass]
public partial class AttackEffect : ActionEffectResource
{
    const double _critMultiplier = 1.5;
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
            int calculatedDamage = 0;
            bool didCrit = false;
            
            switch(damageCalculation)
            {
                case BattleConsts.DamageCalculation.Strength:
                {
                    actorController.RunActorSideEffects(user, BattleConsts.TriggerType.OnUserPhysicalAttack);
                    calculatedDamage = Mathf.RoundToInt((baseDamage + user.GetStrength()) * user.GetStrengthModifier());

                    if ((baseCrit * user.GetCritModifier()) > GD.Randi() % 99)
                    {
                        GD.Print("It's a critical hit!");
                        calculatedDamage = Mathf.RoundToInt(calculatedDamage * _critMultiplier);
                        didCrit = true;
                    } 

                    if (user.IsChargeEnabled()) calculatedDamage = Mathf.RoundToInt(calculatedDamage * _chargeMultiplier);
                    break;
                }

                case BattleConsts.DamageCalculation.Elemental:
                {
                    actorController.RunActorSideEffects(user, BattleConsts.TriggerType.OnUserElementalAttack);
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

            GD.Print("AttackEffect - "+calculatedDamage);

            actorController.TakeDamage(target, -calculatedDamage, didCrit);
        } else {
            // Miss
            actorController.ActionMissed(target);
        }
    }

}
