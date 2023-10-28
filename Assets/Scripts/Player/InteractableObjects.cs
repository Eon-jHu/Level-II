using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObjects : CollidableObjects
{
    private bool z_Interacted = false;
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
    }

    protected virtual void OnInteract(PlayerController _player)
    {
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("PLAYER INTERACTED WITH " + name);

            _player.TriggerOnEncountered();
        }
    }
}
