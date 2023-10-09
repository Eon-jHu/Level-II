using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    BattleUnit m_unit;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider hpSlider;

    public void UpdateHUD()
    {
        nameText.text = m_unit.unitName;
        levelText.text = "Lvl " + m_unit.unitLevel;
        hpSlider.maxValue = m_unit.maxHP;
        hpSlider.value = m_unit.currentHP;
        hpText.text = m_unit.currentHP + "";
    }

    public void LinkHUD(BattleUnit _unit)
    {
        m_unit = _unit;
        UpdateHUD();
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
