using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Equipment/MaxHealth/IncreaseMaxHealthRate")]
public class IncreaseMaxHealthRate : ItemEffect_Equipment
{
    public override void InitEffectValue(QualityType _quality)
    {
        switch (_quality)
        {
            case QualityType.Blue:
                effectValue = 6;
                break;
            case QualityType.Purple:
                effectValue = 10;
                break;
            case QualityType.Gold:
                effectValue = 15;
                break;
        }
    }

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        player.stats.maxHealthRate.AddModifier(10);
    }

    public override void StopEffect()
    {
        player.stats.maxHealthRate.RemoveModifier(10);
    }

    public override string GetEffectDescription()
    {
        return "提升" + effectValue + "%血量上限";
    }
}
