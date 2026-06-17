using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Equipment/AttackRecover/AttackHealByMaxHealth")]
public class AttackHealByMaxHealth : ItemEffect_Equipment
{
    [Range(0,1f)]
    [SerializeField] private float healPercent;

    public override void InitEffectValue(QualityType _quality)
    {
        switch (_quality)
        {
            case QualityType.Blue:
                effectValue = 1;
                break;
            case QualityType.Purple:
                effectValue = 2;
                break;
            case QualityType.Gold:
                effectValue = 3;
                break;
        }
    }

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        player.stats.onDamageExecuteEffect += DamageEffect;
    }

    public override void StopEffect()
    {
        player.stats.onDamageExecuteEffect -= DamageEffect;
    }

    protected override void DamageEffect(CharacterStats _stats, int _damage)
    {
        int amount = Mathf.RoundToInt(player.stats.maxHealth * effectValue * .01f * healPercent);
        amount = Mathf.Clamp(amount, 1, int.MaxValue);
        player.stats.IncreaseHealthBy(amount);
    }

    public override string GetEffectDescription()
    {
        return "¹¥»÷ÃüÖÐÊ±£¬»Ö¸´" + effectValue + "%ÑªÁ¿";
    }
}
