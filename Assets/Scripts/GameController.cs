using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle,
    WorldFlip
}


public class GameController : MonoBehaviour
{
    // variables.
    XPBar ExpBar;
    GrayScale MakeGray;
    CameraShake ShakeCam;

    GameState m_State;
    [SerializeField] PlayerController m_PlayerController;
    [SerializeField] BattleSystem m_BattleSystem;
    [SerializeField] Camera m_WorldCamera;
    [SerializeField] XPBar m_XPBar;
    [SerializeField] CameraShake m_CameraShake;
    [SerializeField] AudioManager m_AudioManager;

    // Subscribe to the created event
    private void Start()
    {
        // Observer/Subscriber Pattern
        m_PlayerController.OnEncountered += StartBattle;
        m_BattleSystem.OnBattleOver += EndBattle;
        m_XPBar.OnXPNotch += StartWorldFlip;
        m_CameraShake.OnShakeOver += EndWorldFlip;
    }

    void StartBattle()
    {
        Debug.Log("StartBattle triggered...");

        m_State = GameState.Battle;
        m_BattleSystem.gameObject.SetActive(true);
        m_WorldCamera.gameObject.SetActive(false);
        m_XPBar.gameObject.SetActive(false);

        // Change Music
        m_BattleSystem.Begin();
        m_AudioManager.SetMusic(GameState.Battle);
    }
    private void EndBattle(float _xp)
    {
        m_State = GameState.FreeRoam;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(true);

        // Update XP Bar
        m_XPBar.UpdateProgress(_xp);
        m_AudioManager.SetMusic(GameState.FreeRoam);
    }
    public void EndWorldFlip()
    {
        m_State = GameState.FreeRoam;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(true);
        m_AudioManager.SetMusic(GameState.FreeRoam);
    }
    void StartWorldFlip()
    {
        Debug.Log("World Flip triggered");

        //ExpBar.UpdateProgress(1.0f);

        // ExpBar.SetWorldFlipIsTriggered(false);
        m_State = GameState.WorldFlip;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(false);

        //// Change Music
        //m_BattleSystem.Begin();
        m_AudioManager.SetMusic(GameState.WorldFlip);

        CameraShake.Instance.ShakeCamera(1.0f, 20.0f);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_State == GameState.FreeRoam || m_State == GameState.WorldFlip)
        {
            m_PlayerController.HandleUpdate();
        }
        else if (m_State == GameState.Battle)
        {
            m_BattleSystem.HandleUpdate();
        }
    }
}
