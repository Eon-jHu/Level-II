using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InteractableObjects : CollidableObjects
{
    private static bool HasSpecialSword = false;

    [SerializeField] public bool IsNotDestroyable = false;
    [SerializeField] private bool IsNotInteractable = false;
    // public GameObject FloatingPoints;
    //[SerializeField] public bool NeedsSpecialSword = false;

    private AudioSource AudioSource;
    public AudioClip BushRustle;

    public bool z_Interacted = false;

    public GameObject dialoguePanel;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayBushRustle()
    {
        if (AudioSource != null)
        {
            AudioSource.clip = BushRustle;
            AudioSource.enabled = true; // Enable the AudioSource before playing
            AudioSource.Play();
            Debug.Log("Audio should have played");
            StartCoroutine(DisableAudioSourceAfterClipEnds());
        }
        else
        {
            Debug.LogError("AudioSource is not assigned!");
        }
    }

    private IEnumerator DisableAudioSourceAfterClipEnds()
    {
        yield return new WaitForSeconds(AudioSource.clip.length);
        AudioSource.enabled = false; // Disable the AudioSource after the clip ends
    }


    public void SetHasSpecialSword(bool _hasSpecialSword)
    {
        HasSpecialSword = _hasSpecialSword;
    }

    protected override void OnCollided(GameObject collidedObject)
    {
        //Debug.Log("HasSpecialSwordBool = " + HasSpecialSword);
        // Try cast the player as the collided Object
        PlayerController player = collidedObject.GetComponent<PlayerController>();

        if (player == null)
        {
            return;
        }

        //if (Input.GetKey(KeyCode.E))
        //{  
        //    OnInteract(player);
        //}

        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == false))
        {
            if (Input.GetMouseButton(0) && !dialoguePanel.activeInHierarchy) // and check for the world flip bool
            {
                OnAttack(player);
            }
        }
    }

    protected virtual void OnInteract(PlayerController _player)
    {
        if (!z_Interacted && !IsNotInteractable)
        {
            z_Interacted = true;

            // Trigger encounter with THIS object
            _player.TriggerOnEncountered(gameObject);

            Destroy(gameObject); // Destroy after interaction.
        }
    }

    protected virtual void OnAttack(PlayerController _player)
    {
        // Make sure it's not already interacted with
        if (z_Interacted)
        {
            return;
        }

        // Destroyable objects simply delete themsleves and grant XP
        if (!IsNotDestroyable)
        {
            PlayBushRustle();
            GameController.instance.m_XPBar.UpdateProgress(2.5f);
            Destroy(gameObject);
            //Instantiate(FloatingPoints, transform.position, Quaternion.identity, transform);
        }
        // Otherwise, you're going to INTERACT with it; in COMBAT
        else
        {
            // TODO: OnInteract has a BOOLEAN, which will be TRUE if they're ATTACKING.
            OnInteract(_player);
        }

        //// Checks if object needs upgraded sword to delete.
        //if (NeedsSpecialSword)
        //{
        //    if (HasSpecialSword)
        //    {
        //        //Debug.Log("HasSpecialSword = " + HasSpecialSword);
        //        Destroy(gameObject);
        //        Debug.Log("Deleted with Special Sword");
        //    }
        //    else if (!HasSpecialSword)
        //    {
        //        //Debug.Log("HasSpecialSword = " + HasSpecialSword);
        //        Debug.Log("You do not yet have enough XP!");
        //        return;
        //    }
        //}

        z_Interacted = true;
    }
}
