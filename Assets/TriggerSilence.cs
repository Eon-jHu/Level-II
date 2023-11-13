using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSilence : MonoBehaviour
{
    private void OnDestroy()
    {
        GameController.instance.m_AudioManager.SetMusic(GameState.FinalRoam);
    }
}
