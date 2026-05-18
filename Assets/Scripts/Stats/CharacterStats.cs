using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private int currentHealth; //当前血量

    [Header("Major Stats")]
    public Stat strength;       //力量    1 increase 1    攻击力     + 1%    暴击伤害  
    public Stat agility;        //敏捷    1 increase 1%   暴击率     
    public Stat intelligence;   //智力    1 increase 1%   形态增伤
    public Stat vitality;       //体质    1 increase 3~5  生命上限

    [Header("Offensive Stats")]                                 
    public Stat damage;         //基础攻击力 
    public Stat critChance;     //暴击率   
    public Stat critPower;      //暴击伤害  

    [Header("Defensive Stats")]
    public Stat maxHealth;      //生命上限
    public Stat armor;          //物理抗性
    public Stat formResistance; //形态抗性  敌人专属   
    public Stat reduction;      //减伤率  1 increase 1%   大概率玩家专属     

    [Header("Buff Stats")]
    public Stat vulnerable;     //易伤    increase 50% damage  
    public Stat weak;           //虚弱    decrease 25% damage
    public Stat breakArmor;     //破甲    decrese  20% armor;
    public Stat dotDamage;      //持续伤害  1 decrease 1 当前血量

    private float dotDamageCooldown = 1f;
    private float dotDamageTimer;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);

        currentHealth = maxHealth.GetValue();
    }
    protected virtual void Update()
    {
        dotDamageTimer -= Time.deltaTime;
        DoDotDamage();
    }


    #region DoDamage
    public virtual void DoDamage(CharacterStats _targetStats, int _damageDir, float _formPercent = 0, FormType _formType = FormType.Neutral)
    {
        float basicValue = damage.GetValue() + strength.GetValue();

        basicValue = CalculateExtraDamage(_targetStats, basicValue);    //易伤 + 虚弱的伤害计算

        float physicalDamage = basicValue * (1f - _formPercent);
        float formDamage = basicValue * _formPercent;

        if (CanCrit())
        {
            physicalDamage = CalculateCriticalDamage(physicalDamage);
            formDamage = CalculateCriticalDamage(formDamage);
        }

        physicalDamage = DoPhysicalDamage(_targetStats, physicalDamage);
        formDamage = DoFormDamage(_targetStats, formDamage, _formType);

        int totalDamage = Mathf.RoundToInt(physicalDamage + formDamage);

        _targetStats.TakeDamage(totalDamage, _damageDir);
    }

    private float CalculateExtraDamage(CharacterStats _targetStats, float basicValue)
    {
        if (weak.GetValue() > 0)
        {
            basicValue *= 0.75f;
        }

        if (_targetStats.vulnerable.GetValue() > 0)
        {
            basicValue *= 1.5f;
        }

        return basicValue;
    }

    public virtual float DoPhysicalDamage(CharacterStats _targetStats, float _physicalDamage)
    {
        if (_physicalDamage <= 0)
            return 0;

        _physicalDamage = CheckTargetArmor(_targetStats, _physicalDamage);

        return _physicalDamage;
    }

    public virtual float DoFormDamage(CharacterStats _targetStats, float _formDamage, FormType _formType)
    {
        return 0f;
    }

    #endregion

    private void DoDotDamage()
    {
        if (dotDamage.GetValue() > 0 && dotDamageTimer < 0)
        {
            int finalDotDamage = dotDamage.GetValue();
            finalDotDamage = Mathf.Clamp(finalDotDamage, 1, int.MaxValue);
            TakeDamage(finalDotDamage);
            dotDamageTimer = dotDamageCooldown;
        }
    }

    public virtual void TakeDamage(int _damage, int _damageDir = 0)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }

    #region Resistance
    protected virtual float CheckTargetArmor(CharacterStats _targetStats, float _physicalDamage)
    {
        if (_targetStats.breakArmor.GetValue() > 0)
            _physicalDamage -= Mathf.Clamp(_targetStats.armor.GetValue() * 0.8f, 0, int.MaxValue);
        else
            _physicalDamage -= Mathf.Clamp(_targetStats.armor.GetValue(), 0, int.MaxValue);

        _physicalDamage = Mathf.Clamp(_physicalDamage, 1, int.MaxValue);
        return _physicalDamage;
    }

    protected virtual float CheckTargetFormResistence(CharacterStats _targetStats, float _formDamage)
    {
        _formDamage -= _targetStats.formResistance.GetValue();
        _formDamage = Mathf.Clamp(_formDamage, 1, int.MaxValue);
        return _formDamage;
    }
    #endregion
    #region Critical Damage
    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue();
        
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private float CalculateCriticalDamage(float _damage)
    {
        float totalCritPower = critPower.GetValue() + strength.GetValue() * .01f;
        float critDamage = _damage * totalCritPower;
        
        return critDamage;
    }
    #endregion
}
