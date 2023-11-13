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
    private bool endOfText = false;
    public Image panelImage;

    // Add a variable to track whether the NPC is in an encounter
    private bool inEncounter = false;

    private string[] initialDialogue; // Store the initial dialogue for resetting

    void Start()
    {
        // Make a copy of the initial dialogue for this NPC
        initialDialogue = new string[dialogue.Length];
        dialogue.CopyTo(initialDialogue, 0);
    }

    void Update()
    {
        if (playerIsClose)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    ZeroText();
                }
                else
                {
                    dialoguePanel.SetActive(true);

                    // Check if this is the start of a new encounter
                    if (!inEncounter)
                    {
                        StartEncounter();
                    }

                    StartCoroutine(Typing());
                }
            }
            if (dialogueText.text == dialogue[index])
            {
                continueButton.SetActive(true);

                if (index == dialogue.Length - 1)
                {
                    endOfText = true;
                }
            }
        }

        if (dialoguePanel.activeInHierarchy && instructionPanel.activeInHierarchy)
        {
            instructionPanel.SetActive(false);
        }
    }

    // Add a function to start a new encounter
    private void StartEncounter()
    {
        inEncounter = true;

        // Reset the dialogue array to its initial state
        dialogue = new string[initialDialogue.Length];
        initialDialogue.CopyTo(dialogue, 0);

        index = 0;
        dialogueText.text = "";
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        // Mark the encounter as ended
        inEncounter = false;
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
        Debug.Log(index);
        continueButton.SetActive(false);
        if (index < (dialogue.Length - 1))
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            endOfText = true;
            ZeroText();

            if (endOfText)
            {
                Debug.Log("End of the text has been reached");

                // Enter combat if engageable
                if (gameObject.GetComponent<Engageable>() != null)
                {
                    GameController.instance.m_PlayerController.TriggerOnEncountered(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionPanel.SetActive(true);
            Debug.Log("Entered proximity");
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionPanel.SetActive(false);
            playerIsClose = false;
            ZeroText();
        }
    }
}
