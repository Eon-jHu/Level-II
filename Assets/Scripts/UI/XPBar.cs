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

    private float target;

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
            Debug.Log("Not able to destroy boxes");
            Sword.SetHasSpecialSword(false);
        }
        // checking stages where the player has enough XP to destroy boxes.
        else if (target >= (37.0f / MaxXP) && target <= (40.0f / MaxXP) ||                         // tutorial boxes enough XP.
            target >= (82.0f / MaxXP) && target <= (85.0f / MaxXP))                            // area 1 boxes enough XP.
        {
            Debug.Log("Able to destroy boxes");
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
        
        target = currentAmount + (amount/MaxXP);
        routine = StartCoroutine(FillRoutine(target, duration));
        Debug.Log("Current XP (XPBar) = " + target);


        // checking for first notch being reached. TODO add more check for different XP bar notches.
        if (target > (39.5f / MaxXP) && target < (40.5f / MaxXP))                                       // tutorial world flip XP.
        {
            // Debug.Log("Notch has been reached");

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
