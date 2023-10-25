using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField]
    XPBar expBar;

    float XPValue = 40.0f;

    // float MaxXP = 240.0f;

    public void TestClick()
    {
        expBar.UpdateProgress(XPValue);
    }
}
