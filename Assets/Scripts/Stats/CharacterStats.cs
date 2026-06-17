using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    health,
    formFocus,
    physicalDamage,
    formDamage,
    critChance,
    critPower
}

public enum EquipmentStatType
{
    strength,    
    intelligence,  
    agility,    
    vitality, 
}

public class CharacterStats : MonoBehaviour
{
    public int currentHealth { get; private set; }      //当前生命
    public int maxHealth { get; private set; }          //最大生命
    public int currentFormFocus { get; private set; }   //当前形态专注值
    public int maxFormFocus { get; private set; }       //最大形态专注值

    [Header("Major Stats")]     //大概率玩家专属
    public Stat strength;       //力量    1 increase 1    物理攻击力     + 1%    暴击伤害  
    public Stat intelligence;   //智力    1 increase 1    形态攻击力     + 1%    形态增伤
    public Stat agility;        //敏捷    1 increase 1%   暴击率     
    public Stat vitality;       //体质    1 increase 3~5  生命上限

    [Header("Offensive Stats")]                                 
    public Stat baseFormFocus;  //基础形态专注值
    public Stat physicalDamage; //物理攻击力
    public Stat formDamage;     //形态攻击力
    public Stat critChance;     //暴击率    only for player
    public Stat critPower;      //暴击伤害  only for player

    [Header("Defensive Stats")]
    public Stat baseHealth;     //基础生命
    public Stat armor;          //物理抗性
    public Stat formResistance; //形态抗性  only for enemy
    public Stat reduction;      //减伤率  1 increase 1%   only for player

    [Header("DamageBuff Stats")]
    public Stat vulnerable;     //易伤    increase 50% damage   only for enemy
    public Stat weak;           //虚弱    decrease 25% damage   only for enemy
    public Stat slowdown;       //减速    1 decrease 1% speed    

    public Stat untargetable;   //无法选中    cannot get DoDamage,but get dotDamage
    public Stat invincible;     //无敌        cannot TakeDamage,including dotDamage,but get damageDebuff

    [Header("DotDamge Stats")]
    public Stat dotDamage;      //持续伤害  1 decrease 1 currentHealth

    private float dotDamageCooldown = 1f;
    private float dotDamageTimer;

    [Header("Improve Rate Stats")]
    public Stat maxHealthRate;
    public Stat maxFormFocusRate;
    public Stat physicalDamageRate;

    public bool isDead {get; private set;}

    #region Action
    public System.Action onHealthChanged;
    public System.Action onFormFocusChanged;
    public System.Action onDamageBuffChanged;
    public System.Action<CharacterStats, int> onDamageExecuteEffect;    //攻击时传递CharacterStats和Damage
    #endregion
    #region Components
    private Entity entity;
    private EntityFx fx;
    #endregion

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFx>();

        vulnerable.onValueChanged += OnAllBuffChanged;      //When Buff Stat changed,Update UI
        weak.onValueChanged += OnAllBuffChanged;
        slowdown.onValueChanged += OnAllBuffChanged;

        baseHealth.onValueChanged += OnAllHealthChanged;    //When Health Stat changed,Update MaxHealth and UI
        vitality.onValueChanged += OnAllHealthChanged;
        maxHealthRate.onValueChanged += OnAllHealthChanged;
        onHealthChanged += SetMaxHealthValue;

        baseFormFocus.onValueChanged += OnAllFormFocusChanged;  //When FormFocus Stat changed,Update MaxFormFocus and UI
        maxFormFocusRate.onValueChanged += OnAllFormFocusChanged;
        onFormFocusChanged += SetMaxFormFocusValue;
    }

    protected virtual void Start()
    {
        vulnerable.SetDefaultValue();
        weak.SetDefaultValue(); 
        slowdown.SetDefaultValue();

        invincible.SetDefaultValue();
        untargetable.SetDefaultValue();
        //以上是Buff类，必须对baseValue进行初始化

        maxHealthRate.SetDefaultValue(100);
        maxFormFocusRate.SetDefaultValue(100);
        physicalDamageRate.SetDefaultValue(100);
        critPower.SetDefaultValue(150);

        currentFormFocus = maxFormFocus;
        currentHealth = maxHealth;
        
        isDead = false;
    }

    protected virtual void Update()
    {
        dotDamageTimer -= Time.deltaTime;
        DoDotDamage();
    }

    #region AddTempModifier
    public void AddTempModifier(Stat _stat, int _modifier, float _seconds) => StartCoroutine(TempModifierCoroutine(_stat, _modifier, _seconds));
    
    private IEnumerator TempModifierCoroutine(Stat _stat, int _modifier, float _seconds)
    {
        _stat.AddModifier(_modifier);
        yield return new WaitForSeconds(_seconds);
        _stat.RemoveModifier(_modifier);
    }
    #endregion
    #region ActionTrigger
    private void OnAllBuffChanged() => onDamageBuffChanged?.Invoke();

    private void OnAllHealthChanged() => onHealthChanged?.Invoke();

    private void OnAllFormFocusChanged() => onFormFocusChanged?.Invoke();
    #endregion
    #region DoDamage
    public virtual void DoDamage(CharacterStats _targetStats, int _damageDir, float _damageRate = 1, float _formPercent = 0, FormType _formType = FormType.Neutral)
    {
        if (_targetStats.untargetable.GetValue() > 0)
            return;

        float physicalValue = (physicalDamage.GetValue() + strength.GetValue()) * physicalDamageRate.GetValue() * .01f;
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

        totalDamage = Mathf.Clamp(totalDamage, 1, int.MaxValue);

        _targetStats.TakeDamage(totalDamage, _damageDir);

        onDamageExecuteEffect?.Invoke(_targetStats, totalDamage);
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
        if (invincible.GetValue() > 0)
            return;

        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);    //防止伤害为负数
        
        DecreaseHealthBy(_damage);

        fx.StartCoroutine("FlashFx");
        entity.DamageImpact(_damageDir);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    #region ChangeHealthBy
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke();
    }
    #endregion
    #region ChangeFormFocusBy
    public virtual void IncreaseFormFocusBy(int _amount)
    {
        currentFormFocus += _amount;
        currentFormFocus = Mathf.Clamp(currentFormFocus, 0, maxFormFocus);

        onFormFocusChanged?.Invoke();
    }

    public virtual bool DecreaseFormFocusBy(int _amount)  //Check if currentFormFocus is enough for cost
    {
        if (_amount > currentFormFocus)
            return false;

        currentFormFocus -= _amount;
        currentFormFocus = Mathf.Clamp(currentFormFocus, 0, maxFormFocus);

        onFormFocusChanged?.Invoke();

        return true;
    }
    #endregion

    protected virtual void Die()
    {
        isDead = true;

        StopAllCoroutines();
        vulnerable.modifiers.Clear();
        weak.modifiers.Clear();
        slowdown.modifiers.Clear();
        
        untargetable.modifiers.Clear();
        invincible.modifiers.Clear();
        
        dotDamage.modifiers.Clear();
    }


    public void SetMaxHealthValue()
    {
        maxHealth = Mathf.RoundToInt( (baseHealth.GetValue() + vitality.GetValue() * 5) * maxHealthRate.GetValue() * .01f);
    }

    public void SetMaxFormFocusValue()
    {
        maxFormFocus = Mathf.RoundToInt(baseFormFocus.GetValue() * maxFormFocusRate.GetValue() * .01f);
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


    public int StatOfType(StatType _statType)
    {
        if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.formFocus) return maxFormFocus;
        else if (_statType == StatType.physicalDamage) return physicalDamage.GetValue() + strength.GetValue();
        else if (_statType == StatType.formDamage) return formDamage.GetValue() + intelligence.GetValue();
        else if (_statType == StatType.critChance) return critChance.GetValue() + agility.GetValue();
        else if (_statType == StatType.critPower) return critPower.GetValue() + strength.GetValue();

        return -1;
    }

    public string StatNameOfType(EquipmentType _equipmentType)
    {
        if (_equipmentType == EquipmentType.Gaunlet) return "力量";
        else if (_equipmentType == EquipmentType.Amulet) return "智力";
        else if (_equipmentType == EquipmentType.Boots) return "敏捷";
        else if (_equipmentType == EquipmentType.Helmet) return "体质";
        else return null;
    }

    private void OnDestroy()
    {
        vulnerable.onValueChanged -= OnAllBuffChanged;
        weak.onValueChanged -= OnAllBuffChanged;
        slowdown.onValueChanged -= OnAllBuffChanged;

        baseHealth.onValueChanged -= OnAllHealthChanged;
        vitality.onValueChanged -= OnAllHealthChanged;
        maxHealthRate.onValueChanged -= OnAllHealthChanged;
        onHealthChanged -= SetMaxHealthValue;

        baseFormFocus.onValueChanged -= OnAllFormFocusChanged;
        maxFormFocusRate.onValueChanged -= OnAllFormFocusChanged;
        onFormFocusChanged -= SetMaxFormFocusValue;

        StopAllCoroutines();
    }
}