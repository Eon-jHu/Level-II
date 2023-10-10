using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    BattleUnit unit;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider ultiSlider;

    public void UpdateHUD()
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        float hpPercentage = ((float)unit.currentHP / (float)unit.maxHP) * 100;
        hpText.text = hpPercentage + "%";

        ultiSlider.maxValue = unit.maxEnergy;
        ultiSlider.value = unit.currentEnergy;
        energyText.text = unit.currentEnergy + "/" + unit.maxEnergy;
    }

    public void LinkHUD(BattleUnit _unit)
    {
        unit = _unit;
        UpdateHUD();
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
