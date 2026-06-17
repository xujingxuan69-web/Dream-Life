using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers = new List<int>();

    public System.Action onValueChanged;

    public int GetValue()       //加算
    {
        int finalValue = baseValue;

        if (modifiers == null)
            return finalValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public float GetMultiValue()   //乘算
    {
        float finalValue = 1;
        if (modifiers == null)
            return finalValue;

        foreach (int modifier in modifiers)
        {
            finalValue *= (100 - modifier) * .01f;
        }

        return finalValue;
    }

    public void SetDefaultValue(int _value = 0)
    {
        baseValue = _value;
        onValueChanged?.Invoke();
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
        onValueChanged?.Invoke();   //.Invoke() 触发委托的所有订阅
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
        onValueChanged?.Invoke();
    }
}
