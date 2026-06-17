using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType    //Equipment词条类型
{ 
    MaxHealth,
    PhysicalDamage,
    GriefDamage,
    WrathDamage,
    CalmDamage,
    CriticalChance,
    CriticalPower,
    HealthDamageReduction,  //满减、损减、血量偏低减伤
    AutoRecover,            //自动恢复
    AttackRecover,          //攻击恢复
    AttackDebuff,           //攻击赋予Debuff
    AttackDot,              //攻击赋予Dot伤害、扩散Dot伤害
    Speed,                  //速度类
}

public class ItemEffect_Equipment : ItemEffect
{
    public EffectType effectType;
    protected int effectValue;    //ItemEffect的作用数值

    public Sprite effectIcon;

    public virtual void InitEffectValue(QualityType _quality)    //根据物品品质设定数值,以下是模板
    {
        /*
        switch (_quality)
        {
            case QualityType.Blue:
                effectValue = Random.Range(0, 100);
                break;
            case QualityType.Purple:
                effectValue = Random.Range(0, 100);
                break;
            case QualityType.Gold:
                effectValue = Random.Range(0, 100);
                break;
        }
        */
    }

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    public override void StopEffect()
    {
        base.StopEffect();
    }

    protected virtual void DamageEffect(CharacterStats _stats, int _damage)
    {
    }

    public virtual string GetEffectDescription()
    {
        return null;
    }
}
