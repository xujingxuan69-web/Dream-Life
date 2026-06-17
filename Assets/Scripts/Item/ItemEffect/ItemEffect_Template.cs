using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Template")]
public class ItemEffect_Template : ItemEffect_Equipment
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

    }

    public override void StopEffect()
    {

    }

    protected override void DamageEffect(CharacterStats _stats, int _damage)
    {
        
    }
}
