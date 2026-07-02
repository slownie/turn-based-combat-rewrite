using Godot;
using System;

[GlobalClass]
public partial class AttackEffect : ActionEffectResource
{
    // Affinity Modifiers
    Godot.Collections.Dictionary<BattleConsts.AffinityType, double> _affinityModifiers = new Godot.Collections.Dictionary<BattleConsts.AffinityType, double>()
    {
        {BattleConsts.AffinityType.Normal, 1.0},
        {BattleConsts.AffinityType.Weak, 1.5},
        {BattleConsts.AffinityType.Resist, 0.5},
        {BattleConsts.AffinityType.Block, 0.0},
    };

    // Buff Modifiers
    const double _critMultiplier = 1.5;
    const int _chargeMultiplier = 3;
    const int _focusMultiplier = 3;

    [Export] int baseDamage = 10;
    [Export] int baseAccuracy = 100;
    [Export] int baseCrit = 21;
    [Export] BattleConsts.DamageCalculation damageCalculation = BattleConsts.DamageCalculation.Strength;
    [Export] BattleConsts.ElementType elementType = BattleConsts.ElementType.Phys;

    [ExportCategory("Attack Flags")]
    [Export] bool statusBonusDamage = false;
    [Export] bool cannotKill = false;
    [Export] bool canCrit = true;

    public override void ExecuteEffect(BattleActor user, BattleActor target, ActorController actorController)
    {
        if (baseAccuracy + Mathf.RoundToInt(user.GetLuck() * user.GetAccuraccyModifier()) > GD.Randi() % 99)
        {
            int calculatedDamage = 0;
            bool didCrit = false;
            
            switch(damageCalculation)
            {
                case BattleConsts.DamageCalculation.Strength:
                {
                    actorController.RunActorSideEffects(user, BattleConsts.TriggerType.OnUserPhysicalAttack);
                    calculatedDamage = Mathf.RoundToInt((baseDamage + user.GetStrength()) * user.GetStrengthModifier());

                    if (canCrit)
                    {
                        if ((baseCrit * user.GetCritModifier()) > GD.Randi() % 99)
                        {
                            actorController.RunActorSideEffects(user, BattleConsts.TriggerType.OnUserDoCritical);
                            GD.Print("It's a critical hit!");
                            calculatedDamage = Mathf.RoundToInt(calculatedDamage * _critMultiplier);
                            didCrit = true;
                        } 
                    }
                    
                    if (user.IsChargeEnabled()) calculatedDamage = Mathf.RoundToInt(calculatedDamage * _chargeMultiplier);

                    int restoreMPAmount = 0;
                    restoreMPAmount = Mathf.RoundToInt(calculatedDamage * 0.5);
                    if (0 < restoreMPAmount) actorController.TakeRejuvenate(user, restoreMPAmount);

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
            if (user.IgnoreAffinity)
            {
                calculatedDamage = Mathf.RoundToInt(calculatedDamage * _affinityModifiers[BattleConsts.AffinityType.Weak]);

                // TESTING
                GD.Print("It pierced through the resistance!");
            } else {
                BattleConsts.AffinityType targetAffinity = target.GetAffinity(elementType);
                double affinityModifier = _affinityModifiers[targetAffinity];
                calculatedDamage = Mathf.RoundToInt(calculatedDamage * affinityModifier);

                // TESTING
                if (targetAffinity == BattleConsts.AffinityType.Weak) GD.Print("Weak!");
                if (targetAffinity == BattleConsts.AffinityType.Resist) GD.Print("Resist");
                if (targetAffinity == BattleConsts.AffinityType.Block) GD.Print("Block");
            }

            // Attack Flags
            if (statusBonusDamage)
            {
                if (target.GetActiveStatusCondition() != null)
                {
                    calculatedDamage = Mathf.RoundToInt(calculatedDamage * _critMultiplier);
                }
            }

            if (cannotKill)
            {
                // Sajam voice: Will it kill?
                if (target.GetCurHP() <= calculatedDamage)
                {
                }
            }

            //GD.Print("AttackEffect - "+calculatedDamage);

            actorController.TakeDamage(user, target, -calculatedDamage, didCrit);
        } else {
            // Miss
            actorController.ActionMissed(target);
        }
    }

}
