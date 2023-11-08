using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUps : MonoBehaviour
{
    public GameObject instructionPanelBush;
    public bool playerIsClose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsClose)
        {
            instructionPanelBush.SetActive(true);
        }
        else
        {
            instructionPanelBush.SetActive(false);
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
            instructionPanelBush.SetActive(false);
        }
    }
}
