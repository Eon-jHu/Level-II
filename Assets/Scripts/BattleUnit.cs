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
    // Background changes for each character
    public Sprite battleBackground;

    public string unitName;
    public int unitLevel;

    // Battle Stats
    public int attackMod;     // Determines attack accuracy
    public int blockMod;      // Determines block effectiveness
    public int maxHP;
    public int currentHP;
    public int maxEnergy;  // Determines ulti charge time
    public int currentEnergy;

    public bool isAlive = true;
    public bool isHit = false;

    // Battle Behaviours
    [SerializeField] AttackBehaviour attackBehaviour;
    [SerializeField] BlockBehaviour blockBehaviour;
    // ChargeBehaviour chargeBehaviour;
    [SerializeField] UltiBehaviour ultiBehaviour;

    [SerializeField] BattleStrategy battleStrategy;

    public EActionType prevAction = EActionType.NONE;
    public EActionType currentAction = EActionType.NONE;

    public void ExecuteBattleStrategy(EActionType _opponentPrevAction)
    {
        currentAction = battleStrategy.Execute(this, _opponentPrevAction);
    }

    // ================= BATTLE BEHAVIOURS =================

    /// <summary>
    /// Resolves an ulti on another BattleUnit
    /// </summary>
    /// <returns>(-1) if not enough energy, else returns damage dealt</returns>
    public int Ulti(BattleUnit _opposingUnit)
    {
        if (currentEnergy >= maxEnergy)
        {
            return ultiBehaviour.ApplyUlti(this, _opposingUnit);
        }
        // NOT enough energy
        {
            return -1;
        }
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

    // Default Parry Behaviour
    public void SucceedBlock()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy++;

            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }

        }
    }

    // Default Charge Behaviour
    public bool Charge()
    {
        if (currentEnergy < maxEnergy && !isHit)
        {
            currentEnergy += 2;

            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
            return true;
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (damage > 0)
        {
            isHit = true;
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
            Debug.Log(unitName + " has died.");
        }
    }

    public void EndPhase(DialogueHelper _dialogueHelper)
    {
        blockMod = 0;
        isHit = false;
    }
}

