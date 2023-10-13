using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField]
    private Text textLevel;

    [SerializeField]
    private Image fillBackground;

    [SerializeField]
    private Color color;

    private int level = 0;

    private float currentAmount = 0;
}
