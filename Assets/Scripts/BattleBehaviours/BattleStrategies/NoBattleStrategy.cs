using UnityEngine;

[CreateAssetMenu(fileName = "NoBattleStrategy", menuName = "Battle Strategies/None")]
public class NoBattleStrategy : BattleStrategy
{
    public override EActionType Execute(BattleUnit _thisUnit, EActionType _opponentPrevAction)
    {
        return EActionType.NONE;
    }
}