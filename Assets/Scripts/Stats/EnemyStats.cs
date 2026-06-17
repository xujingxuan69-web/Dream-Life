using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier;

    protected override void Awake()
    {
        base.Awake();
        vulnerable.onValueChanged += onDamageBuffChanged;
        weak.onValueChanged += onDamageBuffChanged;
    }
    

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(baseHealth);
        Modify(physicalDamage);
    }

    private void Modify(Stat _stat) //!暂定为根据指数递增，等级不能太高，否则容易数值爆炸，后期需要修改根据等级的数值迭代方法
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem?.GenerateDeadDrop(enemy.IsGroundDetected());
    }

    #region Damage
    public override void TakeDamage(int _damage, int _damageDir = 0)
    {
        base.TakeDamage(_damage, _damageDir);
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
    #endregion
}
