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
