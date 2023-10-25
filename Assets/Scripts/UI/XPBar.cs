using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    private Text textLevel;

    [SerializeField]
    private Image fillBar;

    [SerializeField]
    private Image barOutline;

    //[SerializeField]
    //private Color color;

    private int level = 0;

    private float currentAmount = 0;

    private float MaxXP = 240.0f;

    private Coroutine routine;

    void OnEnable()
    {
       // fillBar.color = color;
        level = 0;
        currentAmount = 0;
        fillBar.fillAmount = currentAmount;
        UpdateLevel(level);
    }

    //void InitColor()
    //{
    //    fillBar.color = color;
    //    barOutline.color = color;
    //}

    public void UpdateProgress(float amount, float duration = 0.1f)
    {
        if (routine !=null)
        {
            StopCoroutine(routine);
        }
        float target = currentAmount + (amount/MaxXP);
        routine = StartCoroutine(FillRoutine(target, duration));
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
            LevelUp();
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
}
