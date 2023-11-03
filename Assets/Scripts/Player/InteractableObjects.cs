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

    protected override void OnCollided(GameObject collidedObject)
    {
        if (Input.GetKey(KeyCode.E))
        {
            // Try cast the player as the collided Object
            PlayerController player = collidedObject.GetComponent<PlayerController>();

            // If it IS the player  -> interacting with -> the object
            if (player != null)
            {
                EngageBattle(collidedObject, gameObject);
            }

        }

        if (Input.GetMouseButton(0))
        {
            OnAttack();
        }
    }

    protected void EngageBattle(GameObject _player, GameObject _enemy)
    {
        if (!z_Interacted)
        {
            z_Interacted = true;

            GameController.instance.StartBattle(_player, _enemy);

            Destroy(gameObject); // destroy after interaction.
        }
    }

    protected void OnAttack()
    {
        if (!z_Interacted && !IsNotDestroyable) // only destroy object if destroyable.
        {
            z_Interacted = true;

            GameController.instance.m_XPBar.UpdateProgress(5.0f);

            Destroy(gameObject);
        }

        // Else, start combat with them
    }
}
