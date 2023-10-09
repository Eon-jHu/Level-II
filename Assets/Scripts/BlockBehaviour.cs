using UnityEngine;

public abstract class BlockBehaviour : ScriptableObject
{
    public abstract void Block(BattleUnit _thisUnit);
}
