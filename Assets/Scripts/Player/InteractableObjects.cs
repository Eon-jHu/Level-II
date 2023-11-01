using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObjects : CollidableObjects
{
    private bool z_Interacted = false;

    [SerializeField]
    XPBar expBar;

    [SerializeField] private bool IsNotDestroyable = false;
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
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("Player Interacted With " + name);

            _player.TriggerOnEncountered();
        }
    }

    protected virtual void OnAttack()
    {
        if (!z_Interacted && !IsNotDestroyable) // only destroy is object is destroyable.
        {
            z_Interacted = true;
            Debug.Log("ATTACK");

            expBar.UpdateProgress(10.0f); // on destroy, add 10 XP to bar.
            Debug.Log("UpdatedProgress");

            Destroy(gameObject);
            Debug.Log("DestroyBox");
        }
    }
}
