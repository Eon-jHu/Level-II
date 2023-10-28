using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EBattleState
{
    START,
    READY,
    RESOLVING,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    public EBattleState battleState;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    BattleUnit playerBattleUnit;
    BattleUnit enemyBattleUnit;

    [SerializeField] TextMeshProUGUI dialogueText;
    DialogueHelper dialogueHelper;

    List<string> battleScript = new List<string>();
    List<BattleHUD> battleHUDRefs = new List<BattleHUD>();

    public float xpOnWinBattle = 40.0f; // Default xp for winning a battle
    public event Action<float> OnBattleOver;

    // Called whenever a new battle starts
    public void StartBattle()
    {
        battleState = EBattleState.START;
        StartCoroutine(SetupBattle()); 
    }

    // ================== BATTLE STATE FUNCTIONS ==================

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);

        playerBattleUnit = playerGO.GetComponent<BattleUnit>();
        enemyBattleUnit = enemyGO.GetComponent<BattleUnit>();

        // Initialize HUDs
        playerHUD.LinkHUD(playerBattleUnit);
        enemyHUD.LinkHUD(enemyBattleUnit);

        dialogueHelper = new DialogueHelper();
        dialogueHelper.LinkTextField(dialogueText);

        // Dialogues For Initializing Battle
        dialogueHelper.SetupBattleDialogue(enemyBattleUnit.unitName);
        yield return new WaitForSeconds(1.5f);
        dialogueHelper.ReadyForActionsDialogue();
        yield return new WaitForSeconds(0.5f);

        battleState = EBattleState.READY;
    }

    // TODO: Make into a coroutine.

    void PerformBattle()
    {
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        enemyBattleUnit.ExecuteBattleStrategy(playerBattleUnit.prevAction);

        // Battle Order: Ultis >> Blocks >> Attacks >> Charges >> CleanUp
        ResolveUlti(playerBattleUnit, enemyBattleUnit, enemyHUD);
        ResolveUlti(enemyBattleUnit, playerBattleUnit, playerHUD);

        // Check life after ulti
        if (CheckAlive())
        {
            ResolveBlock(playerBattleUnit);
            ResolveBlock(enemyBattleUnit);

            ResolveAttack(playerBattleUnit, enemyBattleUnit, enemyHUD);
            ResolveAttack(enemyBattleUnit, playerBattleUnit, playerHUD);

            ResolveSuccessfulBlock(playerBattleUnit, enemyBattleUnit, playerHUD);
            ResolveSuccessfulBlock(enemyBattleUnit, playerBattleUnit, enemyHUD);

            ResolveCharge(playerBattleUnit, playerHUD);
            ResolveCharge(enemyBattleUnit, enemyHUD);
        }

        StartCoroutine(ResolveActions());
    }

    private bool CheckAlive()
    {
        if (!playerBattleUnit.isAlive)
        {
            battleState = EBattleState.LOST;
            return false;
        }
        else if (!enemyBattleUnit.isAlive)
        {
            battleState = EBattleState.WON;
            return false;
        }

        return true;
    }

    void ResolveUlti(BattleUnit _thisUnit, BattleUnit _opposingUnit, BattleHUD _opposingBattleHUD)
    {
        if (_thisUnit.currentAction == EActionType.ULTING)
        {
            Debug.Log(_thisUnit.unitName + " ultis");
            int iDamageCheck = _thisUnit.Ulti(_opposingUnit);
            Debug.Log(_thisUnit.unitName + "'s damage = " + iDamageCheck);

            // Add to resolution chain
            battleScript.Add(dialogueHelper.UltiScript(_thisUnit.unitName, iDamageCheck));
            battleHUDRefs.Add(_opposingBattleHUD);
        }
    }

    void ResolveBlock(BattleUnit _unit)
    {
        if (_unit.currentAction == EActionType.BLOCKING)
        {
            Debug.Log(_unit.unitName + " blocks");
            _unit.Block();

            // Add to resolution chain
            battleScript.Add(dialogueHelper.BlockScript(_unit.unitName));
            battleHUDRefs.Add(null);
        }
    }

    void ResolveAttack(BattleUnit _thisUnit, BattleUnit _opposingUnit, BattleHUD _opposingBattleHUD)
    {
        if (_thisUnit.currentAction == EActionType.ATTACKING)
        {
            Debug.Log(_thisUnit.unitName + " attacks");
            int iDamageCheck = _thisUnit.Attack(_opposingUnit);
            Debug.Log(_thisUnit.unitName + "'s damage = " + iDamageCheck);

            // Add to resolution chain
            battleScript.Add(dialogueHelper.AttackScript(_thisUnit.unitName, iDamageCheck));
            battleHUDRefs.Add(_opposingBattleHUD);
        }
    }
    void ResolveSuccessfulBlock(BattleUnit _thisUnit, BattleUnit _opposingUnit, BattleHUD _thisUnitHUD)
    {
        if (_thisUnit.currentAction == EActionType.BLOCKING && _opposingUnit.currentAction == EActionType.ATTACKING)
        {
            // Check for no damage
            if (!_thisUnit.isHit)
            {
                // Apply successful block bonus
                _thisUnit.SucceedBlock();

                // Add to resolution chain
                battleScript.Add(dialogueHelper.SuccessfulBlockScript(_thisUnit.unitName));
                battleHUDRefs.Add(_thisUnitHUD);
            }
        }
    }

    void ResolveCharge(BattleUnit _unit, BattleHUD _unitHUD)
    {
        if (_unit.currentAction == EActionType.CHARGING)
        {
            Debug.Log(_unit + " charges");
            bool isChargeSuccessful = _unit.Charge();

            // Add to resolution chain
            battleScript.Add(dialogueHelper.ChargeScript(_unit.unitName, isChargeSuccessful));
            battleHUDRefs.Add(_unitHUD);
        }
    }

    // ================== VISUAL UPDATE FUNCTIONS ==================

    IEnumerator UpdateBattleHUD(BattleHUD _HUD)
    {
        yield return _HUD.UpdateHUD();
    }

    IEnumerator ResolveActions()
    {
        for (int i = 0; i < battleScript.Count; i++)
        {
            dialogueHelper.TypeDialogue(battleScript[i]);

            if (battleHUDRefs[i] != null)
            {
                UpdateBattleHUD(battleHUDRefs[i]);
            }

            yield return new WaitForSeconds(2f);
        }

        CleanUp();
    }

    private void CleanUp()
    {
        // Clean up step
        playerBattleUnit.EndPhase(dialogueHelper);
        enemyBattleUnit.EndPhase(dialogueHelper);

        // Clear lists
        battleHUDRefs.Clear();
        battleScript.Clear();

        Debug.Log(playerBattleUnit.unitName + " is dead: " + !playerBattleUnit.isAlive);
        Debug.Log(enemyBattleUnit.unitName + " is dead: " + !enemyBattleUnit.isAlive);

        // Check for statuses
        if (CheckAlive())
        {
            // Reset turn
            battleState = EBattleState.READY;
            dialogueHelper.ReadyForActionsDialogue();
        }
        else
        {
            EndBattle();
        }
    }

    private void EndBattle()
    {
        bool bHasPlayerWon = true;

        if (battleState == EBattleState.WON)
        {
            dialogueHelper.TypeDialogue("You won the battle!");
        }
        else if (battleState == EBattleState.LOST)
        {
            dialogueHelper.TypeDialogue("You were defeated...");
            bHasPlayerWon = false;
            xpOnWinBattle = 0.0f;
        }

        StartCoroutine(ReturnToWorld(bHasPlayerWon));
    }

    IEnumerator ReturnToWorld(bool _hasPlayerWon)
    {
        yield return new WaitForSeconds(2f);
        OnBattleOver.Invoke(xpOnWinBattle);
    }

    // ================== BUTTON FUNCTIONS ==================

    public void OnAttackButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.ATTACKING;

        PerformBattle();
    }

    public void OnBlockButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.BLOCKING;

        PerformBattle();
    }

    public void OnChargeButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.CHARGING;

        PerformBattle();
    }

    public void OnUltiButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.ULTING;

        PerformBattle();
    }

    // ================== OTHER FUNCTIONS ==================

    // Update is called once per frame
    public void HandleUpdate()
    {
        // No need for Update() since it's turn-based.
    }
}
