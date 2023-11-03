using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHelper : MonoBehaviour
{
    private TextMeshProUGUI textMeshProField;

    public void LinkTextField(TextMeshProUGUI _dialogueText)
    {
        Debug.Log("Linking Text Field...");
        textMeshProField = _dialogueText;
    }

    // ============== TYPING DIALOGUE ==============

    public IEnumerator TypeDialogue(string _dialogueText)
    {
        Debug.Log("Typing...");

        textMeshProField.text = "";

        foreach (var letter in _dialogueText.ToCharArray())
        {
            textMeshProField.text += letter;
            yield return new WaitForSeconds(1f / 30);
        }
    }

    // ============== BATTLE DIALOGUE ==============

    public IEnumerator SetupBattleDialogue(string _enemyUnitName)
    {
        yield return TypeDialogue("The " + _enemyUnitName + " approaches...");
    }

    public IEnumerator ReadyForActionsDialogue()
    {
        yield return TypeDialogue("You're in battle.\nWhat will you do?");
    }

    public string UltiScript(string _unitName, int _damage)
    {
        if (_damage < 0)
        {
            return _unitName + " tries to Ulti, but doesn't have enough energy.";
        }
        else
        {
            return _unitName + " ULTIS, DEALING " + _damage + " DAMAGE!!!";
        }
    }

    public string BlockScript(string _unitName)
    {
        return _unitName + " blocks.";
    }

    public string AttackScript(string _unitName, int _damage)
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

    public string SuccessfulBlockScript(string _unitName)
    {
        return _unitName + " gains the upper hand!\nThey gain some energy.";
    }

    public string ChargeScript(string _unitName, bool _hasCharged)
    {
        if (_hasCharged)
        {
            return _unitName + " is charging...";
        }
        else
        {
            return _unitName + " tries to charge, but is interrupted!";
        }
    }
}
