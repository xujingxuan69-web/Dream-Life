using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Equipment/Speed/SlowdownEnemy")]
public class DamageSlowdownEnemy : ItemEffect_Equipment
{
    public override void InitEffectValue(QualityType _quality)
    {
        switch (_quality)
        {
            case QualityType.Blue:
                effectValue = 5;
                break;
            case QualityType.Purple:
                effectValue = 8;
                break;
            case QualityType.Gold:
                effectValue = 12;
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
        base.StopEffect();
        player.stats.onDamageExecuteEffect -= DamageEffect;
    }

    protected override void DamageEffect(CharacterStats _stats, int _damage)
    {
        _stats.AddTempModifier(_stats.slowdown, effectValue, 3f);
    }

    public override string GetEffectDescription()
    {
        return "攻击命中时，降低敌人"+effectValue+"%速度";
    }
}
