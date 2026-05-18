using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerStats : CharacterStats
{
    private Player player;

    [Header("Form Stats")]
    public Stat wrathDamage;
    public Stat griefDamage;
    public Stat calmDamage;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, int _damageDir)
    {
        base.TakeDamage(_damage, _damageDir);

        player.DamageEffect(_damageDir);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }

    public override float DoFormDamage(CharacterStats _targetStats, float _formDamage, FormType _formType)
    {
        if (_formDamage <= 0)
            return 0;

        _formDamage = CalculateFormDamage(_formDamage, _formType);
        _formDamage = CheckTargetFormResistence(_targetStats, _formDamage);

        return _formDamage;
    }

    private float CalculateFormDamage(float _formDamage, FormType _formType)
    {
        if (_formType == FormType.Neutral)
        {
            Debug.Log("Wrong Form");
            return 0;
        }

        switch (_formType)
        {
            case FormType.Wrath:
                _formDamage *= 1 + wrathDamage.GetValue() * .01f;
                break;
            case FormType.Grief:
                _formDamage *= 1 + griefDamage.GetValue() * .01f;
                break;
            case FormType.Calm:
                _formDamage *= 1 + calmDamage.GetValue() * .01f;
                break;
            case FormType.Joy:
                _formDamage *= 1 + (wrathDamage.GetValue() + griefDamage.GetValue() + calmDamage.GetValue()) * .01f;
                break;
        }

        _formDamage *= 1 + intelligence.GetValue() * .01f;

        return _formDamage;
    }
}
