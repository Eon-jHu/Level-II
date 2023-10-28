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

    public IEnumerator UpdateHUD()
    {
        // ----- ----- ----- UPDATING HP ----- ----- -----
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        float hpPercentage = ((float)unit.currentHP / (float)unit.maxHP) * 100;
        hpText.text = hpPercentage + "%";
        // Smoothly update the HP bar
        yield return ChangeValueSmoothly(hpSlider, hpSlider.value, (float)unit.currentHP);


        // ----- ----- ----- UPDATING ENERGY ----- ----- -----
        ultiSlider.maxValue = unit.maxEnergy;
        energyText.text = unit.currentEnergy + "/" + unit.maxEnergy;

        // Smoothly update the energy bar
        yield return ChangeValueSmoothly(ultiSlider, ultiSlider.value, (float)unit.currentEnergy);

    }

    public IEnumerator LinkHUD(BattleUnit _unit)
    {
        unit = _unit;
        yield return UpdateHUD();
    }

    private IEnumerator ChangeValueSmoothly(Slider _slider, float _currentValue, float _finalValue)
    {
        float fChangeHP = _currentValue - _finalValue;

        // While the changing HP values isn't equal to the input hp value
        while (fChangeHP > Mathf.Epsilon)
        {
            // Animate HP change a small amount per frame
            _slider.value -= fChangeHP * Time.deltaTime;
            yield return null;
        }

        // Finally, sets the hp to the input value after it's done animating
        _slider.value = _finalValue;
    }
}
