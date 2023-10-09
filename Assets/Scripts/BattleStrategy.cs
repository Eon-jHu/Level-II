using UnityEngine;

public abstract class BattleStrategy : ScriptableObject
{
    public abstract EActionType Execute(EActionType _thisPrevAction, EActionType _opponentPrevAction);
}
