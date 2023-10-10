using UnityEngine;

[CreateAssetMenu(fileName = "BasicBlockBehaviour", menuName = "Action Behaviours/Basic Block")]
public class BasicBlockBehaviour : BlockBehaviour
{
    public override void Block(BattleUnit _thisUnit)
    {
        // Base Values
        const int iBaseBlockValue = 10;
        const int iVarianceLow = -3;
        const int iVarianceHigh = 4;

        // Apply Block
        int iBlockValue = iBaseBlockValue + Random.Range(iVarianceLow, iVarianceHigh);

        _thisUnit.blockMod += iBlockValue; 
    }
}