using UnityEngine;

public abstract class AttackBehaviour : ScriptableObject
{
    // Checks if an attack hits an opposing unit.
    public abstract bool CheckAttack(BattleUnit _thisUnit, BattleUnit _opposingUnit);

    // Attacks an opposing unit. Returns damage dealt.
    public abstract int ApplyAttack(BattleUnit _thisUnit, BattleUnit _opposingUnit);
}