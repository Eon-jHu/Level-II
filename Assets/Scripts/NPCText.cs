using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCText : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public GameObject instructionPanel;
    public string[] dialogue;
    public int index;
    public GameObject continueButton;
    public float wordSpeed;
    public bool playerIsClose;

    void Update()
    {
        if (playerIsClose)
        {
            instructionPanel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    ZeroText();
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                }
            }
            if (dialogueText.text == dialogue[index])
            {
                continueButton.SetActive(true);
            }

        }
        else
        {
            instructionPanel.SetActive(false);
        }

        if (dialoguePanel.activeInHierarchy && instructionPanel.activeInHierarchy)
        {
            instructionPanel.SetActive(false);
        }

    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }
    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }
    public void NextLine()
    {
        continueButton.SetActive(false);
        if (index < dialogue.Length)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ZeroText();
        }
    }
}
