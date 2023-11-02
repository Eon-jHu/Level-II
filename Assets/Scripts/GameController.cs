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
    [SerializeField] XPBar ExpBar;
    GrayScale MakeGray;
    CameraShake ShakeCam;

    GameState m_State;
    [SerializeField] PlayerController m_PlayerController;
    [SerializeField] BattleSystem m_BattleSystem;
    [SerializeField] Camera m_WorldCamera;
    [SerializeField] XPBar m_XPBar;
    [SerializeField] CameraShake m_CameraShake;
    [SerializeField] AudioManager m_AudioManager;
    [SerializeField] InteractableObjects m_Enemies;

    private float tempXP;
    private float newXP;

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
        //Debug.Log("StartBattle triggered...");

        m_State = GameState.Battle;
        m_BattleSystem.gameObject.SetActive(true);
        m_WorldCamera.gameObject.SetActive(false);

        // Store the xp as a tempory value
        tempXP = m_XPBar.GetTarget();
        Debug.Log("Target Value Temp Battle Begin: " + tempXP);

        m_XPBar.gameObject.SetActive(false);

        // Change Music
        m_BattleSystem.Begin();
        m_AudioManager.SetMusic(GameState.Battle);
    }
    private void EndBattle(float _xp)
    {
        // --- NOTE values not inputting correctly
        //Debug.Log("XP awarded in battle = " + _xp);

        _xp = (20.0f / 240.0f);

        //Debug.Log("Manual override of XP = " + _xp);
        // --- NOTE values not inputting correctly

        m_State = GameState.FreeRoam;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(true);



        // bool set to false for battle

        // Update XP Bar
        // testing updating XP bar from state as opposed to previous function calls. 
        //ExpBar.UpdateProgress(20.0f); // on win, add 20 XP to bar.
        //Debug.Log("Updated XP Progress in GameController");

        Debug.Log("Target Value Temp Battle End: " + tempXP);
        Debug.Log("XP to pass in from battle win: " + _xp);

        newXP = ((_xp + tempXP) * 240.0f);
     
        Debug.Log("NewXp: " + newXP);

        // Add temp xp + new xp
        m_XPBar.UpdateProgress(newXP);
        m_AudioManager.SetMusic(GameState.FreeRoam);
    }
    public void EndWorldFlip()
    {
        m_State = GameState.FreeRoam;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(true);

        // updating XP again
        float ThresholdXp = (1.0f / 240.0f); // xp to take bar out of world flip range.
        newXP = ((ThresholdXp + tempXP) * 240.0f);
        m_XPBar.UpdateProgress(newXP);

        // m_Enemies.gameObject.SetActive(true);
        m_AudioManager.SetMusic(GameState.FreeRoam);
    }
    void StartWorldFlip()
    {
        //Debug.Log("World Flip triggered");

        //ExpBar.UpdateProgress(1.0f);

        // ExpBar.SetWorldFlipIsTriggered(false);

        tempXP = m_XPBar.GetTarget();

        m_State = GameState.WorldFlip;
        m_BattleSystem.gameObject.SetActive(false);
        m_WorldCamera.gameObject.SetActive(true);
        m_XPBar.gameObject.SetActive(false);
        //m_Enemies.gameObject.SetActive(false);

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
