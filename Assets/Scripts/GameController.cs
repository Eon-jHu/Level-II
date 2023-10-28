using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle
}


public class GameController : MonoBehaviour
{
    GameState m_State;
    [SerializeField] PlayerController m_PlayerController;
    [SerializeField] BattleSystem m_BattleSystem;
    [SerializeField] Camera m_WorldCamera;
    [SerializeField] XPBar m_XPBar;
    [SerializeField] AudioManager m_AudioManager;

    // Subscribe to the created event
    private void Start()
    {
        // Observer/Subscriber Pattern
        m_PlayerController.OnEncountered += StartBattle;
        m_BattleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        m_State = GameState.Battle;
        m_BattleSystem.gameObject.SetActive(true);
        m_WorldCamera.gameObject.SetActive(false);

        m_BattleSystem.StartBattle();
        m_AudioManager.SetMusic(GameState.Battle);
    }
    private void EndBattle(float _xp)
    {
        m_State = GameState.FreeRoam;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);

        // Update XP Bar
        m_XPBar.UpdateProgress(_xp);
        m_AudioManager.SetMusic(GameState.FreeRoam);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_State == GameState.FreeRoam)
        {
            m_PlayerController.HandleUpdate();
        }
        else if (m_State == GameState.Battle)
        {
            m_BattleSystem.HandleUpdate();
        }
    }
}
