using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 5;

    private Vector2 movement;
    private Rigidbody2D rb;

    private Animator animator;

    // Start the BattleSystem
    public delegate void OnEncounterHandler(GameObject _encountered);
    public event OnEncounterHandler OnEncountered;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Looks for rigid body.
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        // Only update animations if moving; otherwise will reset to Cecil_Up_Idle
        if (movement != Vector2.zero)
        {
            animator.SetFloat("XInput", movement.x);
            animator.SetFloat("YInput", movement.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    // Trigger OnEncountered
    public void TriggerOnEncountered(GameObject _engageable)
    {
        if (_engageable.GetComponent<Engageable>() == null)
        {
            return;
        }
           
        OnEncountered?.Invoke(_engageable.GetComponent<Engageable>().BattleOrInteractionPrefab); 
    }

    public void HandleUpdate() // For physics based & rigid body
    {
        // Variant 1:
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);     

        // Variant 2:
        //if (movement.x != 0 || movement.y != 0)
        //{
        //    rb.velocity = movement * speed;
        //}

        // Variant 3:
        // rb.AddForce(movement * speed);
        
    }
}
