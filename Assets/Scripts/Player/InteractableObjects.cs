using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObjects : CollidableObjects
{
    private bool z_Interacted = false;


    //[SerializeField]
    //XPBar expBar;

    [SerializeField] private bool IsNotDestroyable = false;
    [SerializeField] private bool IsNotInteractable = false;

    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKey(KeyCode.E))
        {
            // Try cast the player as the collided Object
            PlayerController player = collidedObject.GetComponent<PlayerController>();

            if (player != null)
            {
                OnInteract(player);
            }

        }

        if (Input.GetMouseButton(0))
        {
            OnAttack();
        }
    }

    protected virtual void OnInteract(PlayerController _player)
    {
        if (!z_Interacted && !IsNotInteractable)
        {
            z_Interacted = true;

            _player.TriggerOnEncountered(); // enter battle scene.

            Destroy(gameObject); // destroy after interaction.
        }
    }

    protected virtual void OnAttack()
    {
        if (!z_Interacted && !IsNotDestroyable) // only destroy is object is destroyable.
        {
            z_Interacted = true;

            expBar.GetComponent<XPBar>().UpdateProgress(5.0f); // on destroy, add 5 XP to bar.

            // play audio

            Destroy(gameObject);
        }
    }
}
