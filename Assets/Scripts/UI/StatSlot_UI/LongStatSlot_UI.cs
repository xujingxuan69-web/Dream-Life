using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongStatSlot_UI : StatSlot_UI
{
    [SerializeField] private Slider baseValueSlider;
    [SerializeField] private Slider extraValueSlider;

    [SerializeField] private int sliderMaxValue;

    private void OnValidate()
    {
        statNameText.text = statName;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateStatValueUI()
    {
        if (playerStats == null)
            playerStats = PlayerManager.instance.player.stats;

        if (playerStats != null)
        {
            int statValue = playerStats.StatOfType(statType);
            statValueText.text = statValue.ToString();

            baseValueSlider.maxValue = sliderMaxValue;
            extraValueSlider.maxValue = sliderMaxValue;

            extraValueSlider.value = statValue;
            
            if (statType == StatType.health)
            {
                baseValueSlider.value = playerStats.baseHealth.GetValue();
            }
            else if (statType == StatType.formFocus)
            {
                baseValueSlider.value = playerStats.baseFormFocus.GetValue();
            }
        }
    }
}
