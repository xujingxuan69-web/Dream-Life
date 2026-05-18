using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    

    protected override void Start()
    {
        base.Start();
        
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage, int _damageDir = 0)
    {
        base.TakeDamage(_damage, _damageDir);

        enemy.DamageEffect(_damageDir);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }

    public override float DoPhysicalDamage(CharacterStats _targetStats, float _physicalDamage)
    {
        if (_physicalDamage <= 0)
            return 0;

        _physicalDamage = CalculatePhysicalDamage(_targetStats, _physicalDamage);
        _physicalDamage = CheckTargetArmor(_targetStats, _physicalDamage);

        return _physicalDamage;
    }

    private float CalculatePhysicalDamage(CharacterStats _targetStats, float _physicalDamage)
    {
        float damageReduction = _targetStats.reduction.GetMultiValue();
        _physicalDamage = _physicalDamage * damageReduction;

        return _physicalDamage;
    }
}
