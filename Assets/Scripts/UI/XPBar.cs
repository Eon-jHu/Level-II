using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    private Text textLevel;

    [SerializeField]
    private Image fillBar;

    [SerializeField]
    private Image barOutline;

    private bool WorldFlipIsTriggered = false;

    // initiate world flip.
    public event Action OnXPNotch;

    //[SerializeField]
    //private Color color;

    private int level = 0;

    private float currentAmount = 0;

    private float MaxXP = 240.0f;

    private Coroutine routine;

    private float target
    {
        get; set;
    }
    private InteractableObjects Sword;

    void Awake()
    {
        Sword = FindObjectOfType<InteractableObjects>();
    }

    // setters and getters.
    public void SetWorldFlipIsTriggered(bool _TrueOrFalse)
    {
        WorldFlipIsTriggered = _TrueOrFalse;
    }

    void OnEnable()
    {
       // fillBar.color = color;
        level = 0;
        currentAmount = 0;
        fillBar.fillAmount = currentAmount;
        UpdateLevel(level);
    }

    public void Update()
    {
        // checking stages where player does not have enough XP to destroy boxes.
        if (target < (37.0f / MaxXP) ||                                                       // tutorial boxes illegal XP.
           ((target > (85.0f / MaxXP)) && (target < (102.5f / MaxXP))))                       // area 1 boxes illegal XP.
        {
           // Debug.Log("Not able to destroy boxes");
            Sword.SetHasSpecialSword(false);
        }
        // checking stages where the player has enough XP to destroy boxes.
        else if (target >= (37.0f / MaxXP) && target <= (40.0f / MaxXP) ||                         // tutorial boxes enough XP.
            target >= (82.0f / MaxXP) && target <= (85.0f / MaxXP))                            // area 1 boxes enough XP.
        {
            //Debug.Log("Able to destroy boxes");
            Sword.SetHasSpecialSword(true);
        }



    }
    //void InitColor()
    //{
    //    fillBar.color = color;
    //    barOutline.color = color;
    //}

    public void UpdateProgress(float amount, float duration = 0.1f)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        
        if (!GameController.instance.IsInWorldFlip)
        {
            target = currentAmount + (amount / MaxXP);
            routine = StartCoroutine(FillRoutine(target, duration));
            Debug.Log("Current XP (XPBar) = " + target);
        }

        // checking for first notch being reached. TODO add more check for different XP bar notches.
        if (target > (39.5f / MaxXP) && target < (40.5f / MaxXP))
        {
            // Debug.Log("Notch has been reached");
            TriggerOnXPNotch(); // enter world flip.
        }

        // first world flip for area 1.
        if (target > (78.0f / MaxXP) && target < (79.0f / MaxXP))
        {
            TriggerOnXPNotch(); // enter world flip.
        }

        // second world flip for area 1.
        if (target > (118.0f / MaxXP) && target < (120.0f / MaxXP))
        {
            TriggerOnXPNotch(); // enter world flip.
        }

        // World flip for cave area 2.
        if (target > (158.0f / MaxXP) && target < (160.5f / MaxXP))
        {
            TriggerOnXPNotch(); // enter world flip.
        }

        // World flip 1 for final area 3.
        if (target > (199.0f / MaxXP) && target < (201.6f / MaxXP))
        {
            TriggerOnXPNotch(); // enter world flip.
        }

        // Final World Flip.
        if (target > (226.0f / MaxXP))
        {
            TriggerOnXPNotch(); // enter world flip.
        }

    }

    private IEnumerator FillRoutine(float target, float duration)
    {
        float time = 0;
        float tempAmount = currentAmount;
        float diff = target - tempAmount;
        currentAmount= target;

        while (time < duration)
        {
            time += Time.deltaTime;
            float percent = time / duration;
            fillBar.fillAmount = tempAmount + diff * percent;
            yield return null;
        }

        if (currentAmount >= 1)
        {
            //LevelUp();
        }
    }

    // trigger OnXPNotch
    public void TriggerOnXPNotch()
    {
        if (OnXPNotch != null)
        {
            OnXPNotch.Invoke();
        }
    }

    private void LevelUp()
    {
        UpdateLevel(level + 1);
        UpdateProgress(-1f, 0.2f);
    }

    private void UpdateLevel(int level)
    {
        this.level = level;
    }
    
    public float GetTarget()
    {
        return target;
    }
}
