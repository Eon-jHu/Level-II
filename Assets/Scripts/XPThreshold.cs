using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPThreshold : MonoBehaviour
{
    [SerializeField] float XPValueThreshold;

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
            Debug.Log(GameController.instance.m_XPBar.GetTarget() + " > " + (XPValueThreshold / 240.0f));
        }
    }
}
