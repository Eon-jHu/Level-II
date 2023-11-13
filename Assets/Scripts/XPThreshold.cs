using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPThreshold : MonoBehaviour
{
    [SerializeField] float XPValueThreshold;
    public bool playerIsClose;
    public GameObject xpInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.m_XPBar.GetTarget() > (XPValueThreshold / 240.0f))
        {
            // This object becomes not Destroyable
            gameObject.GetComponent<InteractableObjects>().IsNotDestroyable = false;
            gameObject.GetComponent<InteractableObjects>().z_Interacted = false;
            //Debug.Log(GameController.instance.m_XPBar.GetTarget() + " > " + (XPValueThreshold / 240.0f));

        }
        else if (playerIsClose && !GameController.instance.IsInWorldFlip)
        {
            xpInfoPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
           // Debug.Log("Player close");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player no longer close");
            playerIsClose = false;
            xpInfoPanel.SetActive(false);
        }
    }
}
