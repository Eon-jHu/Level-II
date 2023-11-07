using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableObjects : CollidableObjects
{
    private static bool HasSpecialSword = false;

    [SerializeField] private bool IsNotDestroyable = false;
    [SerializeField] private bool IsNotInteractable = false;
    [SerializeField] public bool NeedsSpecialSword = false;

    private bool z_Interacted = false;


    public void SetHasSpecialSword(bool _hasSpecialSword)
    {
        HasSpecialSword = _hasSpecialSword;
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        //Debug.Log("HasSpecialSwordBool = " + HasSpecialSword);
        // Try cast the player as the collided Object
        PlayerController player = collidedObject.GetComponent<PlayerController>();

        if (player == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {  
            OnInteract(player);
        }

        if (Input.GetMouseButton(0))
        {
            OnAttack(player);
        }
    }

    protected virtual void OnInteract(PlayerController _player)
    {
        if (!z_Interacted && !IsNotInteractable)
        {
            z_Interacted = true;

            // Trigger encounter with THIS object
            _player.TriggerOnEncountered(gameObject);

            Destroy(gameObject); // Destroy after interaction.
        }
    }

    protected virtual void OnAttack(PlayerController _player)
    {
        // Make sure it's not already interacted with
        if (z_Interacted)
        {
            return;
        }

        // Destroyable objects simply delete themsleves and grant XP
        if (!IsNotDestroyable)
        {
            GameController.instance.m_XPBar.UpdateProgress(2.5f);
            Destroy(gameObject);
        }
        // Otherwise, you're going to INTERACT with it; in COMBAT
        else
        {
            // TODO: OnInteract has a BOOLEAN, which will be TRUE if they're ATTACKING.
            OnInteract(_player);
        }

        // Checks if object needs upgraded sword to delete.
        if (NeedsSpecialSword)
        {
            if (HasSpecialSword)
            {
                //Debug.Log("HasSpecialSword = " + HasSpecialSword);
                Destroy(gameObject);
                Debug.Log("Deleted with Special Sword");
            }
            else if (!HasSpecialSword)
            {
                //Debug.Log("HasSpecialSword = " + HasSpecialSword);
                Debug.Log("You do not yet have enough XP!");
                return;
            }
        }

        // TODO: Play audioa

        z_Interacted = true;
    }
}
