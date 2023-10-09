using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public LayerMask solidObjectsLayer;

    private bool isMoving;

    private Vector2 input;

    private void Update()
    {
        if (!isMoving) // Checking if the player is moving.
        {
            // Bound to arrowkeys:
            input.x = Input.GetAxisRaw("Horizontal"); // 1 for right, -1 for left.
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) // Prevents diagonal movement.
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos)); // Only moving if there is no collision.
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // Checking if difference between player current position and target position is <.
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); // Move player towards target position.
            yield return null; // Continues to repeat until the difference between player position and target position is negligible.
        }
        transform.position = targetPos;

        isMoving = false;
    }

    // Checking for collision:
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null) // Checking if there is a solid object.
        {
            return false; // Tile is not walkable.
        }
        else
        {
            return true;
        }
    }

}
