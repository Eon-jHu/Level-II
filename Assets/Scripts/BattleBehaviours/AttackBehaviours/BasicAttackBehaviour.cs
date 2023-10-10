using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttackBehaviour", menuName = "Action Behaviours/Basic Attack")]
public class BasicAttackBehaviour : AttackBehaviour
{
    public override bool CheckAttack(BattleUnit _thisUnit, BattleUnit _opposingUnit)
    {
        // 9/10 Chance to hit
        const int iBaseAccuracy = 10;

        return (Random.Range(0, iBaseAccuracy) > 0);
    }

    public override int ApplyAttack(BattleUnit _thisUnit, BattleUnit _opposingUnit)
    {
        // Base Values
        const int iBaseDamage = 5;
        const int iVarianceLow = -2;
        const int iVarianceHigh = 4;

        // Apply Variance
        int iAttackDamage = iBaseDamage + Random.Range(iVarianceLow, iVarianceHigh) - _opposingUnit.blockMod;

        // If damage is BLOCKED
        if (iAttackDamage <= 0)
        {
            
            iAttackDamage = 0;
        }
        // Apply Total Damage
        _opposingUnit.TakeDamage(iAttackDamage);

        return iAttackDamage;
    }
}