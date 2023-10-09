using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
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

    [SerializeField] TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    BattleUnit playerBattleUnit;
    BattleUnit enemyBattleUnit;

    List<string> battleScript = new List<string>();
    List<BattleHUD> battleHUDRefs = new List<BattleHUD>();

    // Start is called before the first frame update
    void Start()
    {
        battleState = EBattleState.START;
        StartCoroutine(SetupBattle()); 
    }

    // ============== BATTLE STATE FUNCTIONS ==============

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);

        playerBattleUnit = playerGO.GetComponent<BattleUnit>();
        enemyBattleUnit = enemyGO.GetComponent<BattleUnit>();

        // Initialize HUDs
        playerHUD.LinkHUD(playerBattleUnit);
        enemyHUD.LinkHUD(enemyBattleUnit);

        SetupBattleDialogue();
        yield return new WaitForSeconds(1.5f);
        ReadyForActionsDialogue();
        yield return new WaitForSeconds(0.5f);

        battleState = EBattleState.READY;
    }

    // TODO: Make into a coroutine.

    void PerformBattle()
    {
        // Change BattleState
        battleState = EBattleState.RESOLVING;

        enemyBattleUnit.ExecuteBattleStrategy(enemyBattleUnit.prevAction, playerBattleUnit.prevAction);

        // Battle Order: Ultis >> Blocks >> Attacks >> Charges >> CleanUp
        //ResolveUltis(playerBattleUnit, enemyBattleUnit);
        ResolveBlocks();
        ResolveAttacks();
        //ResolveCharges();

        StartCoroutine(ResolveActions());
    }

    void UpdateBattleHUD(BattleHUD _HUD)
    {
        _HUD.UpdateHUD();
    }

    void ResolveBlocks()
    {
        if (playerBattleUnit.currentAction == EActionType.BLOCKING)
        {
            Debug.Log("Player blocks");
            playerBattleUnit.Block();

            // Add to resolution chain
            battleScript.Add(BlockDialogue(playerBattleUnit.unitName));
            battleHUDRefs.Add(enemyHUD); // opposing unit's HP; since no diff
        }

        if (enemyBattleUnit.currentAction == EActionType.BLOCKING)
        {
            Debug.Log("Enemy blocks");
            enemyBattleUnit.Block();

            // Add to resolution chain
            battleScript.Add(BlockDialogue(enemyBattleUnit.unitName));
            battleHUDRefs.Add(playerHUD); // opposing unit's HP; since no diff
        }
    }


    void ResolveAttacks()
    {
        if (playerBattleUnit.currentAction == EActionType.ATTACKING)
        {
            Debug.Log("Player attacks");
            int iDamageCheck = playerBattleUnit.Attack(enemyBattleUnit);
            Debug.Log("Player damage = " + iDamageCheck);

            // Add to resolution chain
            battleScript.Add(AttackDialogue(playerBattleUnit.unitName, iDamageCheck));
            battleHUDRefs.Add(enemyHUD);
        }

        if (enemyBattleUnit.currentAction == EActionType.ATTACKING)
        {
            Debug.Log("Enemy attacks");
            int iDamageCheck = enemyBattleUnit.Attack(playerBattleUnit);
            Debug.Log("Enemy damage = " + iDamageCheck);

            // Add to resolution chain
            battleScript.Add(AttackDialogue(enemyBattleUnit.unitName, iDamageCheck));
            battleHUDRefs.Add(playerHUD);
        }
    }

    IEnumerator ResolveActions()
    {
        for (int i = 0; i < battleScript.Count; i++)
        {
            dialogueText.text = battleScript[i];

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
        playerBattleUnit.blockMod = 0;
        enemyBattleUnit.blockMod = 0;

        // Clear lists
        battleHUDRefs.Clear();
        battleScript.Clear();

        Debug.Log(playerBattleUnit.unitName + " is dead: " + !playerBattleUnit.isAlive);
        Debug.Log(enemyBattleUnit.unitName + " is dead: " + !enemyBattleUnit.isAlive);

        // Check for statuses
        if (!playerBattleUnit.isAlive)
        {   
            battleState = EBattleState.LOST;
            EndBattle();
        }
        else if (!enemyBattleUnit.isAlive)
        {
            battleState = EBattleState.WON;
            EndBattle();
        }
        else
        {
            // Reset turn
            battleState = EBattleState.READY;
            ReadyForActionsDialogue();
        }
    }

    // ============== DIALOGUE FUNCTIONS ==============

    private void SetupBattleDialogue()
    {
        dialogueText.text = "The " + enemyBattleUnit.unitName + " approaches...";
    }

    private void ReadyForActionsDialogue()
    {
        dialogueText.text = "You're in battle.\nWhat will you do?";
    }

    private string BlockDialogue(string _unitName)
    {
        return _unitName + " blocks.";
    }

    private string AttackDialogue(string _unitName, int _damage)
    {
        if (_damage < 0)
        {
            return _unitName + " missed on their attack.";
        }
        else if (_damage == 0)
        {
            return _unitName + " dealt no damage on their attack.";
        }
        else
        {
            return _unitName + "'s attack hits for " + _damage + " damage!";
        }
    }

    private void EndBattle()
    {
        if (battleState == EBattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (battleState == EBattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }

    // ============== BUTTON FUNCTIONS ==============

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

    // ============== OTHER FUNCTIONS ==============

    // Update is called once per frame
    void Update()
    {
        // No need for Update() since it's turn-based.
    }
}
