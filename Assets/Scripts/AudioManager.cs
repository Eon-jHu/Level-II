using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioClip[] m_WorldMusic;
    [SerializeField] AudioClip[] m_WorldFlippedMusic;
    [SerializeField] AudioClip[] m_BattleMusic;

    public void SetMusic(GameState _gameState)
    {
        switch (_gameState)
        {
            case GameState.FreeRoam:
            { 
                m_AudioSource.clip = m_WorldMusic[0];
                m_AudioSource.Play();
                    m_AudioSource.volume = 1f;
                    break;
            }

            case GameState.Battle:
            {
                m_AudioSource.clip = m_BattleMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 0.09f;
                break;
            }

            case GameState.WorldFlip:
            {
                m_AudioSource.clip = m_WorldFlippedMusic[0];
                m_AudioSource.Play();
                    m_AudioSource.volume = 1f;
                    break;
            }
        }
    }
}
