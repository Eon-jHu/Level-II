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
            OnInteract();
        }

        if (Input.GetMouseButton(0))
        {
            OnAttack();
        }
    }

    protected virtual void OnInteract()
    {
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("INTERACT WITH " + name);
            SceneManager.LoadScene("BattleScene"); //, LoadSceneMode.Additive);
        }
        
    }

    protected virtual void OnAttack()
    {
        if (!z_Interacted)
        {
            z_Interacted = true;
            Debug.Log("ATTACK");
            Destroy(gameObject);
        }

    }
}
