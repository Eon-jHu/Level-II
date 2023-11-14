using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioClip[] m_WorldMusic;
    [SerializeField] AudioClip[] m_WorldFlippedMusic;
    [SerializeField] AudioClip[] m_BattleMusic;
    [SerializeField] AudioClip[] m_FinalMusic;
    [SerializeField] AudioClip[] m_FinalRoamMusic;

    public bool dontChangeMusic = false;

    public void SetMusic(GameState _gameState)
    {
        if (m_AudioSource == null)
        {
            return;
        }

        switch (_gameState)
        {
            case GameState.FreeRoam:
            { 
                m_AudioSource.clip = m_WorldMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 0.5f;
                break;
            }

            case GameState.Battle:
            {
                m_AudioSource.clip = m_BattleMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 0.05f;
                break;
            }

            case GameState.WorldFlip:
            {
                m_AudioSource.clip = m_WorldFlippedMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 0.75f;
                break;
            }

            case GameState.FinalBattle:
            {
                m_AudioSource.clip = m_FinalMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 1.5f;
                break;
            }

            case GameState.FinalRoam:
            {
                m_AudioSource.clip = m_FinalRoamMusic[0];
                m_AudioSource.Play();
                m_AudioSource.volume = 0.03f;
                break;
            }
        }
    }
}
