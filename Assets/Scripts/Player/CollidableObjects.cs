using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObjects : MonoBehaviour
{
    private Collider2D z_Collider;
    [SerializeField] private ContactFilter2D z_Filter;
    private List<Collider2D> z_CollidedObjects = new List<Collider2D>(1); // Only stores one object for collision.
    [SerializeField] private DialogueHelper dialogueHelper;

    protected virtual void Start()
    {
        z_Collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        z_Collider.OverlapCollider(z_Filter, z_CollidedObjects);
        foreach (var o in z_CollidedObjects)
        {
            OnCollided(o.gameObject);
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        //// checking if the game is in the world flipped state.
        //if (GameController.instance != null && (GameController.instance.IsInWorldFlip == true))
        //{
        //    // checking that collision is only with player.
        //    if (collidedObject.name == "Player_Overworld")
        //    {
        //        StartCoroutine(dialogueHelper.TypeDialogue("It seems as though there is a dead body here."));
        //        //Debug.Log("Collided with " + collidedObject.name);
        //    }
        //}
    }


}
