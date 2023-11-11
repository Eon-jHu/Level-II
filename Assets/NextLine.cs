using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NextLine : MonoBehaviour
{
    public void NextDialogueLine()
    {
        ContactFilter2D filter = new ContactFilter2D();
        // filter.SetLayerMask(LayerMask.GetMask("SolidsObjects")); // Specify the layer(s) you want to check
        Collider2D[] otherCollisionObjects = new Collider2D[20];

        int numOfCollidedObjects = GameController.instance.m_PlayerController.gameObject.GetComponent<Collider2D>().
            OverlapCollider(filter, otherCollisionObjects);



        for (int i = 0; i < numOfCollidedObjects; i++)
        {
            otherCollisionObjects[i].gameObject.GetComponent<NPCText>()?.NextLine();
        }
    }
}
