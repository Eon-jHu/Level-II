using UnityEngine;

[CreateAssetMenu(fileName = "RandomBattleStrategy", menuName = "Battle Strategies/Random")]
public class RandomBattleStrategy : BattleStrategy
{
    public override EActionType Execute(BattleUnit _thisUnit, EActionType _opponentPrevAction)
    {
        int iActionType = -1;

        if (_thisUnit.currentEnergy >= _thisUnit.maxEnergy)
        {
            // Perform Ulti
            iActionType = (int)EActionType.ULTING;
        }
        else
        {
            // Random Move
            iActionType = Random.Range(0, 3);
        }

        Debug.Log("The enemy is " + (EActionType)iActionType);
        return (EActionType)iActionType;
    }
}