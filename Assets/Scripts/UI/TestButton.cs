using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    [SerializeField]
    XPBar expBar;

    public void TestClick()
    {
        expBar.UpdateProgress(0.1f);
    }
}
