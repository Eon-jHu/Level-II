using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EActionType
{
    ATTACKING,
    BLOCKING,
    CHARGING,
    ULTING,
    RUNNING,
    NONE
}

// A PreFab of an enemy or player will have its own BattleUnit component which determines its capabilities in combat
public class BattleUnit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int maxHP;
    public int currentHP;
    public bool isAlive = true;

    // Battle Stats
    public int attackMod;     // Determines attack accuracy
    public int blockMod;      // Determines block effectiveness
    public int chargeLevels;  // Determiknes ulti charge time

    public BattleHUD unitHUD;

    // Battle Behaviours
    [SerializeField] AttackBehaviour attackBehaviour;
    [SerializeField] BlockBehaviour blockBehaviour;
    // ChargeBehaviour chargeBehaviour;
    // UltBehaviour ultiBehaviour;

    [SerializeField] BattleStrategy battleStrategy;

    public EActionType prevAction = EActionType.NONE;
    public EActionType currentAction = EActionType.NONE;

    public void ExecuteBattleStrategy(EActionType _selfPrevAction, EActionType _opponentPrevAction)
    {
        currentAction = battleStrategy.Execute(_selfPrevAction, _opponentPrevAction);
    }

    /// <summary>
    /// Checks and resolves an attack on another BattleUnit
    /// </summary>
    /// <returns>(-1) if attack missed, else returns damage dealt</returns>
    public int Attack(BattleUnit _opposingUnit)
    {
        // Hits
        if (attackBehaviour.CheckAttack(this, _opposingUnit))
        {
            return attackBehaviour.ApplyAttack(this, _opposingUnit);
        }
        // Does NOT hit
        else
        {
            return -1;
        }
    }

    public void Block()
    {
        blockBehaviour.Block(this);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
            Debug.Log(unitName + " has died.");
        }
    }
}
