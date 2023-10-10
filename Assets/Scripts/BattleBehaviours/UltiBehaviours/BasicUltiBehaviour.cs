using UnityEngine;


[CreateAssetMenu(fileName = "BasicUltiBehaviour", menuName = "Action Behaviours/Basic Ulti")]
public class BasicUltiBehaviour : UltiBehaviour
{
    public override int ApplyUlti (BattleUnit _thisUnit, BattleUnit _opposingUnit)
    {
        // Base Values
        const int iBaseDamage = 25;
        const int iVarianceLow = -5;
        const int iVarianceHigh = 6;

        // Apply Variance
        int iDamage = iBaseDamage + Random.Range(iVarianceLow, iVarianceHigh);

        // Apply Total Damage
        _opposingUnit.TakeDamage(iDamage);

        return iDamage;
    }
}