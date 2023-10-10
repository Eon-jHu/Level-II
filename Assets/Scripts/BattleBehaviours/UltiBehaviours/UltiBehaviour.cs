using UnityEngine;

public abstract class UltiBehaviour : ScriptableObject
{
    // Attacks an opposing unit. Returns damage dealt.
    public abstract int ApplyUlti(BattleUnit _thisUnit, BattleUnit _opposingUnit);
}