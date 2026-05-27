using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int currentHealth { get; private set; }//当前血量

    [Header("Major Stats")]     //大概率玩家专属
    public Stat strength;       //力量    1 increase 1    物理攻击力     + 1%    暴击伤害  
    public Stat intelligence;   //智力    1 increase 1    形态攻击力     + 1%    形态增伤
    public Stat agility;        //敏捷    1 increase 1%   暴击率     
    public Stat vitality;       //体质    1 increase 3~5  生命上限

    [Header("Offensive Stats")]                                 
    public Stat physicalDamage; //物理攻击力 
    public Stat formDamage;     //形态攻击力
    public Stat critChance;     //暴击率    玩家专属
    public Stat critPower;      //暴击伤害  玩家专属

    [Header("Defensive Stats")]
    public Stat maxHealth;      //生命上限
    public Stat armor;          //物理抗性
    public Stat formResistance; //形态抗性  敌人专属   
    public Stat reduction;      //减伤率  1 increase 1%   大概率玩家专属     

    [Header("DamageBuff Stats")]
    public Stat vulnerable;     //易伤    increase 50% damage   敌人专属
    public Stat weak;           //虚弱    decrease 25% damage   敌人专属
    public Stat slowdown;       //减速    1 decrease 1% speed    

    [Header("DotDamge Stats")]
    public Stat dotDamage;      //持续伤害  1 decrease 1 当前血量

    private float dotDamageCooldown = 1f;
    private float dotDamageTimer;


    public System.Action onHealthChanged;
    public System.Action onDamageBuffChanged;
    protected bool isDead = false;

    private Entity entity;
    private EntityFx fx;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFx>();

        vulnerable.onValueChanged += OnBuffChanged;
        weak.onValueChanged += OnBuffChanged;
        slowdown.onValueChanged += OnBuffChanged;
    }

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        dotDamageTimer -= Time.deltaTime;
        DoDotDamage();

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(AddTempModifier(weak, 10, 2f));
            StartCoroutine(AddTempModifier(vulnerable, 10, 3f));
        }
    }

    public IEnumerator AddTempModifier(Stat _stat, int _modifier, float _seconds)   //必须用对应的Entity进行StartCoroutine调用
    {
        _stat.AddModifier(_modifier);
        yield return new WaitForSeconds(_seconds);
        _stat.RemoveModifier(_modifier);
    }

    private void OnBuffChanged() => onDamageBuffChanged?.Invoke();

    #region DoDamage
    public virtual void DoDamage(CharacterStats _targetStats, int _damageDir, float _damageRate = 1, float _formPercent = 0, FormType _formType = FormType.Neutral)
    {
        float physicalValue = physicalDamage.GetValue() + strength.GetValue();
        float formValue = formDamage.GetValue() + intelligence.GetValue();

        physicalValue = CalculateExtraDamage(_targetStats, physicalValue);    //易伤 + 虚弱的伤害计算
        formValue = CalculateExtraDamage(_targetStats, formValue);

        float physicalTotallDamage = physicalValue * (1f - _formPercent) * _damageRate;
        float formTotalDamage = formValue * _formPercent * _damageRate;

        if (CanCrit())
        {
            physicalTotallDamage = CalculateCriticalDamage(physicalTotallDamage);
            formTotalDamage = CalculateCriticalDamage(formTotalDamage);
        }

        physicalTotallDamage = DoPhysicalDamage(_targetStats, physicalTotallDamage);
        formTotalDamage = DoFormDamage(_targetStats, formTotalDamage, _formType);

        int totalDamage = Mathf.RoundToInt(physicalTotallDamage + formTotalDamage);

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
        if (dotDamage.GetValue() > 0 && dotDamageTimer < 0 && !isDead)
        {
            int finalDotDamage = dotDamage.GetValue();
            finalDotDamage = Mathf.Clamp(finalDotDamage, 1, int.MaxValue);
            TakeDamage(finalDotDamage);
            dotDamageTimer = dotDamageCooldown;
        }
    }

    public virtual void TakeDamage(int _damage, int _damageDir = 0)
    {
        if (entity.isInvincible)
            return;

        DecreaseHealthBy(_damage);

        fx.StartCoroutine("FlashFx");
        entity.DamageImpact(_damageDir);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;

        StopAllCoroutines();
        vulnerable.modifiers.Clear();
        weak.modifiers.Clear();
        slowdown.modifiers.Clear();
        dotDamage.modifiers.Clear();
    }


    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #region Resistance
    protected virtual float CheckTargetArmor(CharacterStats _targetStats, float _physicalDamage)
    {
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
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private float CalculateCriticalDamage(float _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
        
        return critDamage;
    }
    #endregion

    private void OnDestroy()
    {
        vulnerable.onValueChanged -= OnBuffChanged;
        weak.onValueChanged -= OnBuffChanged;
        slowdown.onValueChanged -= OnBuffChanged;
    }
}