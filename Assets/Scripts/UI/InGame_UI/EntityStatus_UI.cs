using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityStatus_UI : MonoBehaviour
{
    private RectTransform myTransform;
    private CharacterStats myStats;
    private Entity entity;
    private Slider slider;
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private GameObject vulnerableFxPrefab;
    [SerializeField] private GameObject weakFxPrefab;

    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        myStats = GetComponentInParent<CharacterStats>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        UpdateHealthUI();
        UpdateBuffTrigger();
        myStats.onDamageBuffChanged += UpdateBuffTrigger;
    }


    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.maxHealth;
        slider.value = myStats.currentHealth;

        if (slider.value <= 0 || slider.maxValue == slider.value)
            sliderPrefab.SetActive(false);
        else
            sliderPrefab.SetActive(true);
    }
    private void UpdateBuffTrigger()
    {
        if (myStats.vulnerable.GetMultiValue() < 1)
            vulnerableFxPrefab.SetActive(true);
        else
            vulnerableFxPrefab.SetActive(false);

        if (myStats.weak.GetMultiValue() < 1)
            weakFxPrefab.SetActive(true);
        else
            weakFxPrefab.SetActive(false);

        float slowdownRate = 1 - myStats.slowdown.GetMultiValue();
        slowdownRate = Mathf.Clamp(slowdownRate, 0, 0.99f);
        if (slowdownRate > 0)
            entity.SpeedSlowBy(slowdownRate);
        else if (entity.anim.speed != 0)    //!slowdown和timeFreeze会出现冲突，因此加上Clamp限制以区分slowdown与timeFreeze
            entity.SpeedReturnDefault();
    }
    
    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
