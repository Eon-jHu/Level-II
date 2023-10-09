using UnityEngine;

[CreateAssetMenu(fileName = "NoBattleStrategy", menuName = "Battle Strategies/None")]
public class NoBattleStrategy : BattleStrategy
{
    public override EActionType Execute(EActionType _thisPrevAction, EActionType _opponentPrevAction)
    {
        return EActionType.NONE;
    }
}