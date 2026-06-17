using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlot_UI : MonoBehaviour
{
    [SerializeField] protected string statName;
    [SerializeField] protected StatType statType;

    [SerializeField] protected Text statNameText;
    [SerializeField] protected Text statValueText;

    protected PlayerStats playerStats;

    private void OnValidate()
    {
        statNameText.text = statName;
    }

    protected virtual void Start()
    {
        statNameText.text = statName;
        UpdateStatValueUI();
    }

    public virtual void UpdateStatValueUI()
    {
        if (playerStats == null)
            playerStats = PlayerManager.instance.player.stats;

        if (playerStats != null)
        {
            int statValue = playerStats.StatOfType(statType);
            string displayText = statValue.ToString();
            if (statType == StatType.critChance || statType == StatType.critPower)
                displayText += "%";

            statValueText.text = displayText;
        }
    }
}
