using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWorldFlipMusic : MonoBehaviour
{
    private void OnDestroy()
    {
        GameController.instance.m_AudioManager.SetMusic(GameState.WorldFlip);
        GameController.instance.m_AudioManager.dontChangeMusic = true;
    }
}
