using UnityEngine;

public abstract class BattleStrategy : ScriptableObject
{
    public abstract EActionType Execute(BattleUnit _thisUnit, EActionType _opponentPrevAction);
}
