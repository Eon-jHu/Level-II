using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScale : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float duration = 1.0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    public void StartGrayScaleRoutine()
    {
        StartCoroutine(GrayscaleRoutine(duration, true));
    }
    private IEnumerator GrayscaleRoutine(float duration, bool isGrayscale)
    {
        float time = 0;

        while (duration>time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time/ duration;
            float grayAmount = ratio;
            SetGrayScale(grayAmount);
            time += durationFrame;
            yield return null;
        }
        SetGrayScale(1);
    }

    public void SetGrayScale(float _Amount = 1)
    {
        spriteRenderer.material.SetFloat("_GrayScaleAmount", _Amount);
    }
}
