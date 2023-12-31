using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 5;

    public AudioSource bushSoundEffect;
    public AudioSource swordSoundEffect;
    public AudioSource interactAudioEffect;

    private Vector2 movement;

    private Rigidbody2D rb;

    private Animator animator;

    public GameObject dialoguePanel;

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
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.165f);
        Debug.Log("Coroutine");
        animator.SetBool("IsAttacking", false);
    }
    private void OnAttack(InputValue value)
    {
        if (value.isPressed && !animator.GetBool("IsAttacking") && !dialoguePanel.activeInHierarchy && GameController.instance.State == GameState.FreeRoam)
        {
            animator.SetBool("IsAttacking", true);
            swordSoundEffect.Play();

            StartCoroutine(Pause());
        }
    }

    // Trigger OnEncountered
    public void TriggerOnEncountered(GameObject _engageable)
    {
        if (_engageable.GetComponent<Engageable>() == null)
        {
            return;
        }

        if (_engageable.tag == "FinalBoss")
        {
            GameController.instance.m_AudioManager.SetMusic(GameState.FinalBattle);
        }

        if (_engageable.tag == "Timmy(Final)")
        {
            GameController.instance.m_AudioManager.dontChangeMusic = false;
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
