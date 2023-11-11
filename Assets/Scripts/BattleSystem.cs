using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    [SerializeField] XPBar expBar;

    public EBattleState battleState;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    GameObject playerGO;
    GameObject enemyGO;

    [SerializeField] SpriteRenderer backgroundRenderer;
    BattleUnit playerBattleUnit;
    BattleUnit enemyBattleUnit;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] DialogueHelper dialogueHelper;

    List<string> battleScript = new List<string>();
    List<BattleHUD> battleHUDRefs = new List<BattleHUD>();

    public float xpOnWinBattle = 20.0f; // Default xp for winning a battle
    public event Action<float> OnBattleOver;

    // Called whenever a new battle starts
    public void Begin(GameObject _playerPrefab, GameObject _enemyPrefab)
    {
        battleState = EBattleState.START;
        StartCoroutine(SetupBattle(_playerPrefab, _enemyPrefab)); 
    }

    // ================== BATTLE STATE FUNCTIONS ==================

    IEnumerator SetupBattle(GameObject _playerPrefab, GameObject _enemyPrefab)
    {
        //Debug.Log("Setting up battle...");

        playerGO = Instantiate(_playerPrefab, playerBattleStation);
        enemyGO = Instantiate(_enemyPrefab, enemyBattleStation);

        playerBattleUnit = playerGO.GetComponent<BattleUnit>();
        enemyBattleUnit = enemyGO.GetComponent<BattleUnit>();

        // Change the background to the one set on the prefab
        backgroundRenderer.sprite = enemyBattleUnit.battleBackground;

        // Initialize HUDs
        StartCoroutine(playerHUD.LinkHUD(playerBattleUnit));
        StartCoroutine(enemyHUD.LinkHUD(enemyBattleUnit));

        dialogueHelper.LinkTextField(dialogueText);

        // Dialogues For Initializing Battle
        StartCoroutine(dialogueHelper.SetupBattleDialogue(enemyBattleUnit.unitName));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ReadyForActions());
    }

    IEnumerator ReadyForActions()
    {
        StartCoroutine(dialogueHelper.ReadyForActionsDialogue());
        battleState = EBattleState.READY;
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator PerformBattle()
    {
        yield return new WaitForSeconds(1.25f);
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        // Execute Strategy
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
            //Debug.Log(_thisUnit.unitName + " ultis");
            int iDamageCheck = _thisUnit.Ulti(_opposingUnit);
            //Debug.Log(_thisUnit.unitName + "'s damage = " + iDamageCheck);

            // Add to resolution chain
            battleScript.Add(dialogueHelper.UltiScript(_thisUnit.unitName, iDamageCheck));
            battleHUDRefs.Add(_opposingBattleHUD);
        }
    }

    void ResolveBlock(BattleUnit _unit)
    {
        if (_unit.currentAction == EActionType.BLOCKING)
        {
           // Debug.Log(_unit.unitName + " blocks");
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
            //Debug.Log(_thisUnit.unitName + " attacks");
            int iDamageCheck = _thisUnit.Attack(_opposingUnit);
            //Debug.Log(_thisUnit.unitName + "'s damage = " + iDamageCheck);

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
            //Debug.Log(_unit + " charges");
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
            StartCoroutine(dialogueHelper.TypeDialogue(battleScript[i]));

            if (battleHUDRefs[i] != null)
            {
                StartCoroutine(UpdateBattleHUD(battleHUDRefs[i]));
            }

            yield return new WaitForSeconds(2f);
        }

        StartCoroutine(CleanUp());
    }

    private IEnumerator CleanUp()
    {
        // Clean up step
        playerBattleUnit.EndPhase(dialogueHelper);
        enemyBattleUnit.EndPhase(dialogueHelper);

        // Clear lists
        battleHUDRefs.Clear();
        battleScript.Clear();

       // Debug.Log(playerBattleUnit.unitName + " is dead: " + !playerBattleUnit.isAlive);
       // Debug.Log(enemyBattleUnit.unitName + " is dead: " + !enemyBattleUnit.isAlive);

        // Check for statuses
        if (CheckAlive())
        {
            // Reset turn
            StartCoroutine(ReadyForActions());
        }
        else
        {
            EndBattle();
        }

        yield return null;
    }

    public void ResetTheGame()
    {
        //SceneManager.LoadSceneAsync("BattleScene");
        SceneManager.LoadSceneAsync("MainWorld");
        Debug.Log("Game has been reset");
    }

    private void EndBattle()
    {
        bool bHasPlayerWon = true;

        if (battleState == EBattleState.WON)
        {
            StartCoroutine(dialogueHelper.TypeDialogue("You won the battle!"));

            //// testing updating XP bar from battle as opposed to previous function calls.     // ineffective in current game state?
            //expBar.UpdateProgress(20.0f); // on win, add 20 XP to bar.
            //Debug.Log("Updated XP Progress in Battle System");
        }
        else if (battleState == EBattleState.LOST)
        {
            StartCoroutine(dialogueHelper.TypeDialogue("You were defeated... Restarting Cecils Journey."));
            ResetTheGame();
            bHasPlayerWon = false;
            xpOnWinBattle = 0.0f;
        }


        if (enemyGO)
        {
            Destroy(enemyGO);
        }

        StartCoroutine(ReturnToWorld(bHasPlayerWon));
    }

    IEnumerator ReturnToWorld(bool _hasPlayerWon)
    {
        yield return new WaitForSeconds(2f);
        OnBattleOver.Invoke(xpOnWinBattle); // this is not carrying over the updated XP I don't think.
    }

    // ================== BUTTON FUNCTIONS ==================

    public void OnAttackButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        //Debug.Log(name + " activated!");

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.ATTACKING;

        StartCoroutine(PerformBattle());
    }

    public void OnBlockButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        //Debug.Log(name + " activated!");

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.BLOCKING;

        StartCoroutine(PerformBattle());
    }

    public void OnChargeButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        //Debug.Log(name + " activated!");

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.CHARGING;

        StartCoroutine(PerformBattle());
    }

    public void OnUltiButton()
    {
        if (battleState != EBattleState.READY)
        {
            return;
        }
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        //Debug.Log(name + " activated!");

        playerBattleUnit.prevAction = playerBattleUnit.currentAction;
        playerBattleUnit.currentAction = EActionType.ULTING;

        StartCoroutine(PerformBattle());
    }

    // ================== OTHER FUNCTIONS ==================

    // Update is called once per frame
    public void HandleUpdate()
    {
        // No need for Update() since it's turn-based.
    }
}
