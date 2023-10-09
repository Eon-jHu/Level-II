using UnityEngine;

[CreateAssetMenu(fileName = "RandomBattleStrategy", menuName = "Battle Strategies/Random")]
public class RandomBattleStrategy : BattleStrategy
{
    public override EActionType Execute(EActionType _thisPrevAction, EActionType _opponentPrevAction)
    {
        int iActionType = Random.Range(0, 2);
        Debug.Log(iActionType + " is the enemy's action.");
        return (EActionType)iActionType;
    }
}