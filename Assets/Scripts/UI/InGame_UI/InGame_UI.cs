using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider healthMaxSlider;
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Slider formFocusMaxSlider;
    [SerializeField] private Slider formFocusSlider;

    [SerializeField] private Text currencyText;
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
        UpdatePlayerUI();
        playerStats.onHealthChanged += UpdatePlayerUI;
        playerStats.onFormFocusChanged += UpdatePlayerUI;
        player.onFormChanged += UpdateFormUI;
    }

    private void Update()
    {
        currencyText.text = PlayerManager.instance.currency.ToString(); //!记得改成委托
    }

    private void UpdatePlayerUI()
    {
        healthMaxSlider.value = playerStats.maxHealth;
        healthSlider.maxValue = playerStats.maxHealth;
        healthSlider.value = playerStats.currentHealth;
        formFocusMaxSlider.value = playerStats.maxFormFocus;
        formFocusSlider.maxValue = playerStats.maxFormFocus;
        formFocusSlider.value = playerStats.currentFormFocus;
        

    }

    private void UpdateFormUI()
    {
        
    }

    private void OnDestroy()
    {
        playerStats.onHealthChanged -= UpdatePlayerUI;
        playerStats.onFormFocusChanged -= UpdatePlayerUI;
        player.onFormChanged -= UpdateFormUI;
    }
}
